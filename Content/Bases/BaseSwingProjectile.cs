using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LunarVeil.Content.Bases
{
    public abstract class BaseSwingProjectile : ModProjectile
    {       
        //This is for smoothin the trail
        public static int ExtraUpdateMult => 8;
     
        protected bool init;
        private Vector2 _oldOwnerPos;
        public float hitstopTimer=0;
        public Player Owner => Main.player[Projectile.owner];

        protected int SwingTime => (int)((SwingTimeFunction() * ExtraUpdateMult) / Owner.GetAttackSpeed(Projectile.DamageType));
        public float holdOffset = 60f;

        //Ending Swing Time so it doesn't immediately go away after the swing ends, makes it look cleaner I think
        public int EndSwingTime = 6 * ExtraUpdateMult;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 90;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 100;
            Projectile.width = 100;
            Projectile.friendly = true;
            Projectile.scale = 1f;
        
            Projectile.extraUpdates = ExtraUpdateMult - 1;
       
            Projectile.usesLocalNPCImmunity = true;

            //Multiplying by the thing so it's still 10 ticks
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawSlashTrail();
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);


            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself

            return false;
        }


        public override bool ShouldUpdatePosition() => false;
        protected abstract void SwingAI();
        
        public sealed override void AI()
        {
            base.AI();
            if(hitstopTimer > 0)
            {
                Projectile.timeLeft++;
                hitstopTimer--;
            }

            SwingAI();
        }

        protected virtual float SwingTimeFunction()
        {
            return 32;
        }

        protected void OvalEasedSwingAI()
        {
            float swingXRadius = 32;
            float swingYRadius = 128;
            float swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
            float lerpValue = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft, true);
            float swingProgress = lerpValue;
            float targetRotation = Projectile.velocity.ToRotation();
            ModifyOvalSwingAI(targetRotation, lerpValue, ref swingXRadius, ref swingYRadius, ref swingRange, ref swingProgress);

            int dir2 = (int)Projectile.ai[1];
            float xOffset;
            float yOffset;
            if (dir2 == -1)
            {
                xOffset = swingXRadius * MathF.Sin(swingProgress * swingRange + swingRange);
                yOffset = swingYRadius * MathF.Cos(swingProgress * swingRange + swingRange);
            }
            else
            {
                xOffset = swingXRadius * MathF.Sin((1f - swingProgress) * swingRange + swingRange);
                yOffset = swingYRadius * MathF.Cos((1f - swingProgress) * swingRange + swingRange);
            }

            Projectile.Center = Owner.Center + new Vector2(xOffset, yOffset).RotatedBy(targetRotation);
            Projectile.rotation = (Projectile.Center - Owner.Center).ToRotation() + MathHelper.PiOver4;
            OrientHand();
        }

        protected void SimpleEasedSwingAI()
        {
            Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
            float multiplier = 0.2f;
            RGB *= multiplier;

            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            int dir = (int)Projectile.ai[1];

            float lerpValue = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft, true);

            //Smooth it some more
            float swingProgress = lerpValue;

            // the actual rotation it should have
            float targetRotation = Projectile.velocity.ToRotation();

            //How wide is the swing, in radians
            float start = 0;
            float end = 0;
            ModifySimpleSwingAI(targetRotation, lerpValue, ref start, ref end, ref swingProgress);
            // current rotation obv
            // angle lerp causes some weird things here, so just use a normal lerp
            float rotation = dir == 1 ? MathHelper.Lerp(start, end, swingProgress) : MathHelper.Lerp(end, start, swingProgress);

            // offsetted cuz sword sprite
            Vector2 position = Owner.RotatedRelativePoint(Owner.MountedCenter);
            position += rotation.ToRotationVector2() * holdOffset;
            Projectile.Center = position;
            Projectile.rotation = (position - Owner.Center).ToRotation() + MathHelper.PiOver4;
            OrientHand();
        }


        protected virtual void ModifyOvalSwingAI(float targetRotation, float lerpValue,
            ref float swingXRadius,
            ref float swingYRadius,
            ref float swingRange,
            ref float swingProgress)
        {
            swingXRadius = 32;
            swingYRadius = 128;
            swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
            swingProgress = Easing.InOutExpo(lerpValue, 5);
        }
      
        protected virtual void ModifySimpleSwingAI(float targetRotation, float lerpValue, 
            ref float startSwingRot, 
            ref float endSwingRot, 
            ref float swingProgress)
        {
            float swingRange = MathHelper.PiOver2 + MathHelper.PiOver4;
            startSwingRot = targetRotation - swingRange;
            endSwingRot = targetRotation + swingRange;
            swingProgress = Easing.InOutBack(lerpValue);
        }

        private void OrientHand()
        {
            float rotation = Projectile.rotation;
            Owner.heldProj = Projectile.whoAmI;
            Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            Owner.itemRotation = rotation * Owner.direction;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;

            // Set composite arm allows you to set the rotation of the arm and stretch of the front and back arms independently
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
            Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); // get position of hand

            armPosition.Y += Owner.gfxOffY;
        }


        private Color ColorFunction(float p)
        {
            return Color.White;
        }

        private float WidthFunction(float p)
        {
            return MathHelper.Lerp(40, 0, p);
        }

        protected virtual BaseShader ReadyShader()
        {
            SimpleGradientTrailShader shader = SimpleGradientTrailShader.Instance;

            //Main trailing texture
            shader.SlashTexture = TextureRegistry.FXSwordSlash;

            //Blends with the main texture
            shader.GradientTexture = TextureRegistry.FXSwordSlashGradientBright;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.PrimaryColor = Color.LightBlue;
            shader.SecondaryColor = Color.Blue;

            //Alpha Blend/Additive
            shader.BlendState = BlendState.Additive;
            return shader;
        }

        private void DrawSlashTrail()
        {
            var shader = ReadyShader();
            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
            Projectile.oldPos,
              Projectile.oldRot,
              ColorFunction,
              WidthFunction, shader, offset: new Vector2(68f / 2f, 72f / 2f));
        }
    }
}

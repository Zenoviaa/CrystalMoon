using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Bases
{
    public abstract class BaseSwingProjectile : ModProjectile
    {       
        //This is for smoothin the trail
        public static int ExtraUpdateMult => 6;

        private bool _init;
        protected float _timer;
        private bool _hashit;
        private bool _hasMissed;
        protected float _smoothedLerpValue;
        private Vector2[] _trailPoints = new Vector2[0];
        public float hitstopTimer=0;
        public Player Owner => Main.player[Projectile.owner];

        protected int SwingTime => (int)(((SwingTimeFunction() * ExtraUpdateMult) / Owner.GetAttackSpeed(Projectile.DamageType)));
        public float holdOffset = 60f;
        public float trailStartOffset = 0.15f;
        public float missTimeIncrease = 12;
        
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
            Projectile.height = 64;
            Projectile.width = 64;
            Projectile.friendly = true;
            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
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

        public override bool? CanDamage()
        {
            return _smoothedLerpValue > 0.1f && _smoothedLerpValue < 0.9f;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 dir = (Projectile.Center - start).SafeNormalize(Vector2.Zero);
            Vector2 end = start + dir * holdOffset * Projectile.scale * 1.8f;
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale, ref collisionPoint);
        }

        public sealed override void AI()
        {
            base.AI();
          
            if (!_init)
            {
                Projectile.timeLeft = SwingTime;
                InitSwingAI();
                _init = true;
            }

            if(hitstopTimer > 0)
            {
                _timer--;
                Projectile.timeLeft++;
                hitstopTimer--;
            }

            if(!_hashit && !_hasMissed && _smoothedLerpValue > 0.9f)
            {
                Projectile.timeLeft += (int)(missTimeIncrease * ExtraUpdateMult);
                _hasMissed = true;
            }
            if(_hasMissed)
            {
      
            }
            SwingAI();
        }

      
        protected virtual void InitSwingAI()
        {

        }

        protected virtual float SwingTimeFunction()
        {
            return 16;
        }

        protected void OvalEasedSwingAI()
        {
            float swingXRadius = 32;
            float swingYRadius = 128;
            float swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;


            _timer++;
            float lerpValue = _timer / SwingTime;
            float swingProgress = lerpValue;
            float targetRotation = Projectile.velocity.ToRotation();
            ModifyOvalSwingAI(targetRotation, lerpValue, ref swingXRadius, ref swingYRadius, ref swingRange, ref swingProgress);
            _smoothedLerpValue = swingProgress;
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

            Vector2[] points = new Vector2[ProjectileID.Sets.TrailCacheLength[Type]];
            for (int i = 0; i < points.Length; i++)
            {
                float l = points.Length;
                //Lerp between the points
                float progressOnTrail = i / l;

                //Calculate starting lerp value
                float startTrailLerpValue = MathHelper.Clamp(lerpValue - trailStartOffset, 0, 1);
                float startTrailProgress = startTrailLerpValue;
                ModifyOvalSwingAI(targetRotation, startTrailLerpValue, 
                    ref swingXRadius, ref swingYRadius, ref swingRange, ref startTrailProgress);

                //Calculate ending lerp value
                float endTrailLerpValue = lerpValue;
                float endTrailProgress = endTrailLerpValue;
                ModifyOvalSwingAI(targetRotation, endTrailLerpValue, 
                    ref swingXRadius, ref swingYRadius, ref swingRange, ref endTrailProgress);


                //Lerp in between points
                float smoothedTrailProgress = MathHelper.Lerp(startTrailProgress, endTrailProgress, progressOnTrail);
                float xOffset2;
                float yOffset2;
                if (dir2 == -1)
                {
                    xOffset2 = swingXRadius * MathF.Sin(smoothedTrailProgress * swingRange + swingRange);
                    yOffset2 = swingYRadius * MathF.Cos(smoothedTrailProgress * swingRange + swingRange);
                }
                else
                {
                    xOffset2 = swingXRadius * MathF.Sin((1f - smoothedTrailProgress) * swingRange + swingRange);
                    yOffset2 = swingYRadius * MathF.Cos((1f - smoothedTrailProgress) * swingRange + swingRange);
                }


                Vector2 pos = Owner.Center + new Vector2(xOffset2, yOffset2).RotatedBy(targetRotation);
                points[i] = pos - (GetFramingSize() / 2);// + GetTrailOffset().RotatedBy(targetRotation);
            }
            _trailPoints = points;
        }

        protected virtual Vector2 GetFramingSize()
        {
            return new Vector2(68, 72);
        }

        protected virtual Vector2 GetTrailOffset()
        {
            return Vector2.One * 30;
        }

        protected void SimpleEasedSwingAI()
        {
            Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
            float multiplier = 0.2f;
            RGB *= multiplier;

            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            int dir = (int)Projectile.ai[1];

            _timer++;
            float lerpValue = _timer / SwingTime;

            //Smooth it some more
            float swingProgress = lerpValue;

            // the actual rotation it should have
            float targetRotation = Projectile.velocity.ToRotation();

            //How wide is the swing, in radians
            float start = 0;
            float end = 0;
            ModifySimpleSwingAI(targetRotation, lerpValue, ref start, ref end, ref swingProgress);
            _smoothedLerpValue = swingProgress;

            // current rotation obv
            // angle lerp causes some weird things here, so just use a normal lerp
            float rotation = dir == 1 ? MathHelper.Lerp(start, end, swingProgress) : MathHelper.Lerp(end, start, swingProgress);
           
            // offsetted cuz sword sprite
            Vector2 position = Owner.RotatedRelativePoint(Owner.MountedCenter);
            position += rotation.ToRotationVector2() * holdOffset;
            Projectile.Center = position;
            Projectile.rotation = (position - Owner.Center).ToRotation() + MathHelper.PiOver4;
            OrientHand();

            //Calculate Trail Points
            Vector2[] points = new Vector2[ProjectileID.Sets.TrailCacheLength[Type]];
            for (int i = 0; i < points.Length; i++)
            {
                float l = points.Length;
                //Lerp between the points
                float progressOnTrail = i / l;

                //Calculate starting lerp value
                float startTrailLerpValue = MathHelper.Clamp(lerpValue - trailStartOffset, 0,1);
                float startTrailProgress = startTrailLerpValue;
                ModifySimpleSwingAI(targetRotation, startTrailLerpValue, ref start, ref end, ref startTrailProgress);

                //Calculate ending lerp value
                float endTrailLerpValue = lerpValue;
                float endTrailProgress = endTrailLerpValue;
                ModifySimpleSwingAI(targetRotation, endTrailLerpValue, ref start, ref end, ref endTrailProgress);


                //Lerp in between points
                float smoothedTrailProgress = MathHelper.Lerp(startTrailProgress, endTrailProgress, progressOnTrail);
                float rot = dir == 1 ? MathHelper.Lerp(start, end, smoothedTrailProgress) : MathHelper.Lerp(end, start, smoothedTrailProgress);

                Vector2 pos = Owner.RotatedRelativePoint(Owner.MountedCenter);
                pos += rot.ToRotationVector2() * GetTrailOffset();
                points[i] = pos - GetFramingSize() / 2;
            }
            _trailPoints = points;
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


        protected virtual Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.Blue, p);
        }

        protected virtual float WidthFunction(float p)
        {
            return MathHelper.Lerp(0, 48, Easing.OutExpo(p, 6));
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
            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
            _trailPoints,
              Projectile.oldRot,
              ColorFunction,
              WidthFunction, ReadyShader(), offset: GetFramingSize() / 2f);
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Color.White * Easing.SpikeOutCirc(1f - _smoothedLerpValue);

            spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            _hashit = true;
        }
    }
}

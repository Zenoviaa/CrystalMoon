using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Bases
{
    public abstract class BaseSwingProjectile : ModProjectile
    {
        private bool _init;
        private bool _hashit;
        private bool _hasMissed;

        //This is for smoothin the trail
        public static int ExtraUpdateMult => 6;
        public Vector2[] _trailPoints = new Vector2[0];
        private List<BaseSwingStyle> _swingStyles = new();
     
        public float Countertimer;
    
        public float _smoothedLerpValue;

        public float hitstopTimer=0;
        public float bounceTimer = 0;
        public Player Owner => Main.player[Projectile.owner];

        protected int ComboDirection => (int)Projectile.ai[1];

        public float holdOffset = 60f;
        public float trailStartOffset = 0.15f;
        public float missTimeIncrease = 12;
        public float extraSwingTime = 0;
        public float hitboxLengthMult = 1;

        public bool thrust;
        public float OvalRotOffset;
        public bool spinCenter;
        public float spinCenterOffset;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 90;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            _trailPoints = new Vector2[90];
            Projectile.timeLeft = 10;
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

        public virtual void SetComboDefaults(List<BaseSwingStyle> swings)
        {

        }

        public float GetSwingTime(float baseSwingTime)
        {
            float swingTime = baseSwingTime * ExtraUpdateMult;
            return (int)(swingTime / Owner.GetAttackSpeed(Projectile.DamageType));
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

        public override bool? CanDamage()
        {
            return _smoothedLerpValue > 0.1f && _smoothedLerpValue < 0.9f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            float length = texture.Width / 2 + texture.Height / 2;

            Vector2 start = Projectile.Center - Projectile.rotation.ToRotationVector2() * length;
            Vector2 end = Projectile.Center + Projectile.rotation.ToRotationVector2() * length;
            float collisionPoint = 0f;
         
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale, ref collisionPoint);
        }

        public override void AI()
        {
            base.AI();
 
            if (!_init)
            {
                SetComboDefaults(_swingStyles);
                var sw = _swingStyles[(int)Projectile.ai[0]];
                Projectile.timeLeft = (int)GetSwingTime(sw.swingTime);
            
                InitSwingAI();
                _init = true;
            }

            BaseSwingStyle swingStyle = _swingStyles[(int)Projectile.ai[0]];
            swingStyle.SwingProjectile = this;
            if (hitstopTimer > 0)
            {
                Countertimer--;
                Projectile.timeLeft++;
                hitstopTimer--;
            } 
            else if(bounceTimer > 0)
            {
                Countertimer-=2;
         
                bounceTimer--;
                if (bounceTimer <= 0)
                {
                    Projectile.ResetLocalNPCHitImmunity();
                }
                extraSwingTime++;
                Projectile.timeLeft++;
            }

            if(!_hashit && !_hasMissed && _smoothedLerpValue > 0.9f)
            {
                Projectile.timeLeft += (int)(missTimeIncrease * ExtraUpdateMult);
                _hasMissed = true;
            }

            Countertimer++;

            swingStyle.AI();
            OrientHand();
        }

      
        protected virtual void InitSwingAI()
        {

        }

        /*
        protected void OvalEasedSwingAI()
        {
            float swingXRadius = 32;
            float swingYRadius = 128;
            float swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;


            Countertimer++;
            float lerpValue = Countertimer / SwingTime;
            float swingProgress = lerpValue;
            float targetRotation = Projectile.velocity.ToRotation();
            ModifyOvalSwingAI(targetRotation, lerpValue, ref swingXRadius, ref swingYRadius, ref swingRange, ref swingProgress);
            _smoothedLerpValue = swingProgress;
            int dir2 = (int)Projectile.ai[1];
            float xOffset;
            float yOffset;
            if (dir2 == -1)
            {
                xOffset = swingXRadius * MathF.Sin(swingProgress * swingRange + swingRange + OvalRotOffset);
                yOffset = swingYRadius * MathF.Cos(swingProgress * swingRange + swingRange + OvalRotOffset);
            }
            else
            {
                xOffset = swingXRadius * MathF.Sin((1f - swingProgress) * swingRange + swingRange + OvalRotOffset);
                yOffset = swingYRadius * MathF.Cos((1f - swingProgress) * swingRange + swingRange + OvalRotOffset);
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
                    xOffset2 = swingXRadius * MathF.Sin(smoothedTrailProgress * swingRange + swingRange + OvalRotOffset);
                    yOffset2 = swingYRadius * MathF.Cos(smoothedTrailProgress * swingRange + swingRange + OvalRotOffset);
                }
                else
                {
                    xOffset2 = swingXRadius * MathF.Sin((1f - smoothedTrailProgress) * swingRange + swingRange + OvalRotOffset);
                    yOffset2 = swingYRadius * MathF.Cos((1f - smoothedTrailProgress) * swingRange + swingRange + OvalRotOffset);
                }


                Vector2 pos = Owner.Center + new Vector2(xOffset2, yOffset2).RotatedBy(targetRotation);
                points[i] = pos - (GetFramingSize() / 2);// + GetTrailOffset().RotatedBy(targetRotation);
            }
            _trailPoints = points;
        }

        protected void StabSwingEasedAI()
        {
            Countertimer++;

            float lerpValue = Countertimer / SwingTime;

            float swingProgress = lerpValue;
            float targetRotation = Projectile.velocity.ToRotation();
            float stabRange = 1;

            ModifyStabSwingAI(targetRotation, lerpValue, ref stabRange, ref swingProgress);
            _smoothedLerpValue = swingProgress;
            float dir2 = (int)Projectile.ai[1];

            Vector2 swingDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Vector2 swingVelocity = swingDirection * stabRange;
            if (!thrust)
            {
                Owner.velocity += swingDirection * 5;
                thrust = true;
            }

            Projectile.Center = Owner.Center +
                Vector2.Lerp(Vector2.Zero, swingVelocity, swingProgress) + swingDirection * holdOffset;

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
                ModifyStabSwingAI(targetRotation, startTrailLerpValue, ref stabRange, ref startTrailProgress);

                //Calculate ending lerp value
                float endTrailLerpValue = lerpValue;
                float endTrailProgress = endTrailLerpValue;
                ModifyStabSwingAI(targetRotation, endTrailLerpValue, ref stabRange, ref endTrailProgress);


                //Lerp in between points
                float smoothedTrailProgress = MathHelper.Lerp(startTrailProgress, endTrailProgress, progressOnTrail);
                Vector2 pos = Owner.Center +
                    Vector2.Lerp(Vector2.Zero, swingVelocity, smoothedTrailProgress) + swingDirection * holdOffset;
                points[i] = pos - (GetFramingSize() / 2); //+ (pos - Owner.RenderPosition).SafeNormalize(Vector2.Zero) * HoldOffset;// - (GetFramingSize() / 2);// + GetTrailOffset().RotatedBy(targetRotation);
            };
            _trailPoints = points;
        }


        protected virtual void ModifyStabSwingAI(float targetRotation, float lerpValue, ref float stabRange, ref float swingProgress)
        {
            stabRange = 32;
            swingProgress = Easing.SpikeOutCirc(lerpValue);
        }*/

        public virtual Vector2 GetFramingSize()
        {
            return new Vector2(68, 72);
        }

        public virtual Vector2 GetTrailOffset()
        {
            return Vector2.One * 30;
        }

        /*

        protected void SimpleEasedSwingAI()
        {
            Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
            float multiplier = 0.2f;
            RGB *= multiplier;

            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            int dir = (int)Projectile.ai[1];

            Countertimer++;
            float lerpValue = Countertimer / SwingTime;

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
            if (spinCenter)
            {
                Projectile.Center -= rotation.ToRotationVector2() * holdOffset;
                Projectile.Center -= rotation.ToRotationVector2() * spinCenterOffset;
            }
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
                if (spinCenter)
                {
                    Vector2 d = (Owner.RotatedRelativePoint(Owner.MountedCenter) - pos).SafeNormalize(Vector2.Zero);
                    pos += d * spinCenterOffset;
                }
                points[i] = pos - GetFramingSize() / 2;
            }
            _trailPoints = points;
        }

        */
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

            float speedXa = -Projectile.velocity.X * Main.rand.NextFloat(.4f, .7f) + Main.rand.NextFloat(-8f, 8f);
            float speedYa = -Projectile.velocity.Y * Main.rand.Next(0, 0) * 0.01f + Main.rand.Next(-20, 21) * 0.0f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, speedXa * 0, speedYa * 0, ModContent.ProjectileType<BaseHitEffect>(), (int)(Projectile.damage * 0), 0f, Projectile.owner, 0f, 0f);
            _hashit = true;
        }
    }
}

using CrystalMoon.Registries;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.ExampleContent.Projectiles
{
    internal class ExampleMoonlightMagicHexProjectile : ModProjectile
    {
        private ref float _timer => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 36;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.light = 0.67f;
        }

        public override void AI()
        {
            base.AI();
       
            _timer++;
            AI_Particles();
        }

        private void AI_Particles()
        {
            if (_timer % 8 == 0)
            {
                for(int i = 0; i < Projectile.oldPos.Length - 1; i++)
                {
                    if (!Main.rand.NextBool(4))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = Projectile.oldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = Projectile.oldPos[i + 1] - Projectile.oldPos[i];
                    velocity = velocity.SafeNormalize(Vector2.Zero) * -8;
                    Particle.NewParticle<SparkleHexParticle>(spawnPoint, velocity, Color.White);
                }
            }
        }

        private Color ColorFunction(float completionRatio)
        {
            Color c = Color.Lerp(Color.White, Color.Transparent, completionRatio);
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            return MathHelper.Lerp(32, 8, completionRatio);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Trail
            var shader = MagicHexShader.Instance;

            shader.PrimaryTexture = TrailRegistry.GlowTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;
            shader.PrimaryColor = new Color(195, 158, 255);
            shader.NoiseColor = new Color(78, 76, 180);//new Color(78, 76, 180);
            shader.OutlineColor = Color.White;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 5.2f;
            shader.Distortion = 0.1f;

            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, Projectile.oldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size/2);
            return base.PreDraw(ref lightColor);
        }
    }
}

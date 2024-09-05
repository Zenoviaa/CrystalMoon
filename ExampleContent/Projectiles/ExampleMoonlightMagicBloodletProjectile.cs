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
    internal class ExampleMoonlightMagicBloodletProjectile : ModProjectile
    {
        private ref float _timer => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 18;
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
            //AI_Particles();
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
            Color c = Color.White;
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            return MathHelper.Lerp(32, 8, completionRatio);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Trail
            var shader = MagicBloodletShader.Instance;

            shader.PrimaryTexture = TextureRegistry.NoiseTextureWater3;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            shader.PrimaryColor = new Color(255, 51, 51);
            shader.NoiseColor = Color.Black;
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 3.5f;
            shader.Distortion = 0.3f;
            shader.Alpha = 0.25f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, Projectile.oldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size/2);
            return base.PreDraw(ref lightColor);
        }
    }
}

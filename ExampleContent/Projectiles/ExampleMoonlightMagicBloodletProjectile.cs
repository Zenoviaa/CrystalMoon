using CrystalMoon.Registries;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Systems.Shaders.MagicTrails;
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
        int trailingMode = 0;
        private ref float Countertimer => ref Projectile.ai[0];
        public override string Texture => TextureRegistry.EmptyTexturePath;
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
            Countertimer++;

            AI_Particles();
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 48);
        }

        private void AI_Particles()
        {
            if (Countertimer % 8 == 0)
            {
                for(int i = 0; i < Projectile.oldPos.Length - 1; i++)
                {
                    if (!Main.rand.NextBool(4))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = Projectile.oldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = Projectile.oldPos[i + 1] - Projectile.oldPos[i];
                    velocity = velocity.SafeNormalize(Vector2.Zero) * -8;

                    Color color = Color.White;
                    color.A = 0;
                    Particle.NewBlackParticle<BloodSparkleParticle>(spawnPoint, velocity, color);
                }
            }
        }

        private Color ColorFunction(float completionRatio)
        {
            Color c = Color.Red;
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            switch (trailingMode)
            {
                default:
                case 0:
                    return MathHelper.Lerp(32, 0, completionRatio);
                case 1:
                    return MathHelper.Lerp(190, 0, completionRatio);
            }
        
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Trail
            var shader = MagicBloodletShader.Instance;
            trailingMode = 0;
            shader.PrimaryTexture = TextureRegistry.BloodletTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            shader.PrimaryColor = new Color(255, 51, 51);
            shader.NoiseColor = Color.Lerp(shader.PrimaryColor, Color.Black, 0.5f);
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 10.5f;
            shader.Distortion = 0.1f;
            shader.Alpha = 0.25f;

            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, Projectile.oldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size/2);

            //trailingMode = 1;
            //shader.Alpha = 0.25f;
            //TrailDrawer.Draw(Main.spriteBatch, Projectile.oldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
            return base.PreDraw(ref lightColor);
        }
    }
}

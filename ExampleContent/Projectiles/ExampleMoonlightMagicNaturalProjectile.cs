using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
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
    internal class ExampleMoonlightMagicNaturalProjectile : ModProjectile
    {
        private int trailMode = 0;
        private ref float _timer => ref Projectile.ai[0];
        public override string Texture => TextureRegistry.EmptyTexturePath;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 48;
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
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            _timer++;
            AI_Particles();
            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) *8, 0.1f);
        }

        private void AI_Particles()
        {      
            if (_timer % 8 == 0)
            {
                for(int i = 0; i < Projectile.oldPos.Length - 1; i++)
                {
                    if (!Main.rand.NextBool(16))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = Projectile.oldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = Projectile.oldPos[i + 1] - Projectile.oldPos[i];
                    velocity = velocity.SafeNormalize(Vector2.Zero) * -2;

                    Color color = Color.White;
                    color.A = 0;
      
                    if (Main.rand.NextBool(1))
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Particle.NewBlackParticle<WhiteFlowerParticle>(spawnPoint, velocity, Color.White);
                                break;
                            case 1:
                                Particle.NewBlackParticle<PurpleFlowerParticle>(spawnPoint, velocity, Color.White);
                                break;
                            case 2:
                                Particle.NewBlackParticle<BlueFlowerParticle>(spawnPoint, velocity, Color.White);
                                break;
                        }
                      
                    }

                    if (Main.rand.NextBool(32))
                    {
                        Particle.NewBlackParticle<MusicParticle>(spawnPoint, velocity, color);
                    }
                }
            }
        }

        private Color ColorFunction(float completionRatio)
        {
            return Color.White; ;
        }

        private float WidthFunction(float completionRatio)
        {
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            return MathHelper.Lerp(0, 64, completionRatio);
        }
            
        private void DrawMainShader()
        {
            //Trail
            trailMode = 0;
            var shader = MagicNaturalShader.Instance;
            shader.PrimaryTexture = TextureRegistry.NoiseTextureLeaves;
            shader.NoiseTexture = TextureRegistry.NoiseTextureLeaves;
            shader.ShapeTexture = TextureRegistry.DottedTrail;
            shader.BlendState = BlendState.AlphaBlend;
            shader.PrimaryColor = new Color(95, 106, 47);
            shader.NoiseColor = Color.White;
            shader.Speed = 0.5f;
            shader.Distortion = 0.1f;
            shader.Threshold = 0.1f;

            //This just applis the shader changes
            //Main Fill
            TrailDrawer.Draw(Main.spriteBatch, Projectile.oldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMainShader();
            return false;
        }
    }
}

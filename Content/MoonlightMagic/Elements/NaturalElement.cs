using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class NaturalElement : BaseElement
    {
        public override Color GetElementColor()
        {
            return ColorUtil.NaturalGreen;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.NaturalGreen);
        }

        public override void AI()
        {
            base.AI();
            AI_Particles();
        }

        private void AI_Particles()
        {
            if (MagicProj.GlobalTimer % 8 == 0)
            {
                for (int i = 0; i < MagicProj.OldPos.Length - 1; i++)
                {
                    if (!Main.rand.NextBool(16))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = MagicProj.OldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = MagicProj.OldPos[i + 1] - MagicProj.OldPos[i];
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
            int width = (int)(MagicProj.Size * 2.5f);
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            return MathHelper.Lerp(0, width, completionRatio);
        }

        private void DrawMainShader()
        {
            //Trail
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
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        public override void DrawTrail()
        {
            base.DrawTrail();
            DrawMainShader();
        }
    }
}

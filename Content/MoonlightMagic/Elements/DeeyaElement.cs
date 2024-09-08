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
    internal class DeeyaElement : BaseElement
    {
        int trailingMode = 0;

        public override Color GetElementColor()
        {
            return ColorUtil.DeeyaPink;
        }


        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.DeeyaPink);
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
                    if (!Main.rand.NextBool(4))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = MagicProj.OldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = MagicProj.OldPos[i + 1] - MagicProj.OldPos[i];
                    velocity = velocity.SafeNormalize(Vector2.Zero) * -8;

                    Color color = Color.White;
                    color.A = 0;
                    Particle.NewBlackParticle<BloodSparkleParticle>(spawnPoint, velocity, color);
                }
            }
        }

        private Color ColorFunction(float completionRatio)
        {
            Color c = Color.White;
            c.R = 0;
            c.G = 0;
            c.B = 0;
            c.A = 0;
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            float width = MagicProj.Size * 2f;
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            switch (trailingMode)
            {
                default:
                case 0:
                    return MathHelper.Lerp(0, width, completionRatio);
                case 1:
                    return MathHelper.Lerp(0, width / 3f, completionRatio);
                case 2:
                    return MathHelper.Lerp(0, width / 2f, completionRatio);
            }
        }

        private void DrawMainShader()
        {
            trailingMode = 0;
            var shader = MagicBloodletShader.Instance;
            trailingMode = 0;


            shader.PrimaryTexture = TextureRegistry.BloodletTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            shader.PrimaryColor = Color.Black;
            shader.NoiseColor = Color.Black;
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 5.5f;
            shader.Distortion = 2.5f;
            shader.Alpha = 0.25f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);

        }
        private void DrawOutlineShader()
        {
            trailingMode = 1;
            var shader = MagicRadianceOutlineShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrailOutline;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;
            Color pink = new Color(255, 59, 247);
            Color c = Color.White;
            c = pink;
            pink = Color.Lerp(pink, Color.Black, 0.5f);
            shader.PrimaryColor = Color.White;
            shader.NoiseColor = pink;
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.8f;
            shader.Distortion = 0.85f;
            shader.Power = 2.5f;

            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        public override void DrawTrail()
        {
            base.DrawTrail();
            DrawMainShader();
            DrawOutlineShader();
        }
    }
}

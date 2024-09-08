using CrystalMoon.Registries;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class HexElement : BaseElement
    {
        public override Color GetElementColor()
        {
            return ColorUtil.HexPurple;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.HexPurple);
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

        public override void DrawTrail()
        {
            base.DrawTrail();
            var shader = MagicHexShader.Instance;

            shader.PrimaryTexture = TextureRegistry.GlowTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;
            shader.PrimaryColor = new Color(195, 158, 255);
            shader.NoiseColor = new Color(78, 76, 180);//new Color(78, 76, 180);
            shader.OutlineColor = Color.White;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 5.2f;
            shader.Distortion = 0.1f;

            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }
    }
}

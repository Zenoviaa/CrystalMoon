using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class BasicElement : BaseElement
    {
        public override void AI()
        {
            AI_Particles();
        }

        public override void DrawTrail()
        {
            DrawMainShader();
        }

        private void AI_Particles()
        {

        }

        public override bool DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset)
        {
            base.DrawTextShader(spriteBatch, item, line, ref yOffset);
            EnchantmentDrawHelper.DrawTextShader(spriteBatch, item, line, ref yOffset,
                glowColor: Color.White,
                primaryColor: Color.White,
                noiseColor: Color.DarkGray);
            return true;
        }

        private Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(Color.Gray, Color.White, completionRatio);
        }

        private float WidthFunction(float completionRatio)
        {
            float width = MagicProj.Size;
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            return MathHelper.Lerp(0, width, completionRatio);
        }

        private void DrawMainShader()
        {
            var shader = MagicNormalShader.Instance;
            shader.PrimaryTexture = TextureRegistry.GlowTrail;
            shader.NoiseTexture = TextureRegistry.SpikyTrail;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.5f;
            shader.Repeats = 1f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }
    }
}

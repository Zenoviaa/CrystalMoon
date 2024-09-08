using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class PhantasmalElement : BaseElement
    {
        public override Color GetElementColor()
        {
            return ColorUtil.PhantasmalGreen;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.PhantasmalGreen);
        }

        public override void AI()
        {
            base.AI();
            AI_Particles();
        }

        private void AI_Particles()
        {

        }

        private Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(new Color(69, 196, 182), Color.SpringGreen, completionRatio);
        }

        private float WidthFunction(float completionRatio)
        {
            float width = MagicProj.Size * 10f;
            float p = Easing.SpikeOutCirc(completionRatio);
            float ep = Easing.InBack(1 - completionRatio);
            return MathHelper.Lerp(0, width, p * ep);
        }

        private void DrawMainShader()
        {
            var shader = MagicPhantasmalShader.Instance;
            shader.PrimaryTexture = TextureRegistry.GlowTrail;
            shader.NoiseTexture = TextureRegistry.SpikyTrail;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.5f;
            shader.Repeats = 1f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        public override void DrawTrail()
        {
            base.DrawTrail();
            DrawMainShader();
        }
    }
}

using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

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

        private Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(Color.Gray, Color.White, completionRatio);
        }

        private float WidthFunction(float completionRatio)
        {
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            return MathHelper.Lerp(0, 24, completionRatio);
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

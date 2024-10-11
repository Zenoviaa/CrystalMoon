using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;

namespace CrystalMoon.Content.Items.Dyes
{
    internal class BurningDyeArmorShaderData : ArmorShaderData
    {
        public BurningDyeArmorShaderData(Ref<Effect> shader, string passName)
            : base(shader, passName)
        {
        }
        public BurningDyeArmorShaderData(Asset<Effect> shader, string passName)
            : base(shader, passName)
        {
        }

        public override void Apply()
        {
            UseColor(Color.OrangeRed);
            UseSecondaryColor(Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f));
            Shader.Parameters["primaryTexture"].SetValue(TextureRegistry.DottedTrail.Value);
            Shader.Parameters["noiseTexture"].SetValue(TextureRegistry.NoiseTextureClouds3.Value);
            Shader.Parameters["outlineTexture"].SetValue(TextureRegistry.DottedTrailOutline.Value);
            UseOpacity(0.2f);
            Shader.Parameters["outlineColor"].SetValue(Color.DarkRed.ToVector3());
            Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.5f);
            Shader.Parameters["uIntensity"].SetValue(1.5f);
            base.Apply();
        }
    }
}

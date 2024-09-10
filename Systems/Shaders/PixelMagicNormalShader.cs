using CrystalMoon.Registries;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class PixelMagicNormalShader : BaseShader
    {
        private static PixelMagicNormalShader _instance;
        public PixelMagicNormalShader()
        {
            Data = ShaderRegistry.PixelMagicNormal;
            NoiseTexture = TextureRegistry.StarTrail;
            Speed = 5;
        }

        public static PixelMagicNormalShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PixelMagicNormalShader();
                return _instance;
            }
        }

        public Asset<Texture2D> NoiseTexture { get; set; }
        public float Speed { get; set; }
        public override void Apply()
        {
            Data.UseImage1(NoiseTexture);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}

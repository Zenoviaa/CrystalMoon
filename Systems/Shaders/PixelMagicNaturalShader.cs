using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class PixelMagicNaturalShader : BaseShader
    {
        private static PixelMagicNaturalShader _instance;
        public PixelMagicNaturalShader()
        {
            Data = ShaderRegistry.PixelMagicNatural;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            ShapeTexture = TextureRegistry.DottedTrail;
            PrimaryColor = Color.White;
            NoiseColor = Color.White;
            Speed = 5;
            Distortion = 0.2f;
            Threshold = 0.3f;
        }

        public static PixelMagicNaturalShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PixelMagicNaturalShader();
                return _instance;
            }
        }

        public Asset<Texture2D> NoiseTexture { get; set; }
        public Asset<Texture2D> ShapeTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color NoiseColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Threshold { get; set; }
        public override void Apply()
        {
            Data.UseImage1(NoiseTexture);
            Data.UseImage2(ShapeTexture);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["threshold"].SetValue(Threshold);
        }
    }
}

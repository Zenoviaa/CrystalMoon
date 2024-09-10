using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class PixelMagicGuutShader : BaseShader
    {
        private static PixelMagicGuutShader _instance;
        public PixelMagicGuutShader()
        {
            Data = ShaderRegistry.PixelMagicGuut;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            OutlineTexture = TextureRegistry.DottedTrailOutline;
            PrimaryColor = Color.White;
            NoiseColor = Color.White;
            OutlineColor = Color.White;
            Speed = 5;
            Distortion = 0.2f;
            Power = 1.5f;
        }

        public static PixelMagicGuutShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PixelMagicGuutShader();
                return _instance;
            }
        }

        public Asset<Texture2D> NoiseTexture { get; set; }
        public Asset<Texture2D> OutlineTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color NoiseColor { get; set; }
        public Color OutlineColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Power { get; set; }
        public override void Apply()
        {
            Data.UseImage1(NoiseTexture);
            Data.UseImage2(OutlineTexture);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["outlineColor"].SetValue(OutlineColor.ToVector3());
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["power"].SetValue(Power);
        }
    }
}

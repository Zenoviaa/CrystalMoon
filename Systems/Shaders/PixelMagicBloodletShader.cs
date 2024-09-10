using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class PixelMagicBloodletShader : BaseShader
    {
        private static PixelMagicBloodletShader _instance;
        public PixelMagicBloodletShader()
        {
            Data = ShaderRegistry.PixelMagicBloodlet;
            NoiseTexture = TextureRegistry.StarTrail;
            PrimaryColor = Color.White;
            NoiseColor = Color.White;
            OutlineColor = Color.White;
            Speed = 5;
            Distortion = 0.2f;
        }

        public static PixelMagicBloodletShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PixelMagicBloodletShader();
                return _instance;
            }
        }
        public Asset<Texture2D> NoiseTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color NoiseColor { get; set; }
        public Color OutlineColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Alpha { get; set; }
        public override void Apply()
        {
            Data.UseImage1(NoiseTexture);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["outlineColor"].SetValue(OutlineColor.ToVector3());
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["alpha"].SetValue(Alpha);
        }
    }
}

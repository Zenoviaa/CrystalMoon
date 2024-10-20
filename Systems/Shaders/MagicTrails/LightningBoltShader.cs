using CrystalMoon.Registries;
using CrystalMoon.Systems.Shaders.Dyes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders.MagicTrails
{
    internal class LightningBoltShader : BaseShader
    {
        private static LightningBoltShader _instance;
        public static LightningBoltShader Instance
        {
            get
            {
                _instance ??= new();
                _instance.SetDefaults();
                return _instance;
            }
        }
        public Asset<Texture2D> PrimaryTexture { get; set; }
        public Asset<Texture2D> NoiseTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color NoiseColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Power { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            PrimaryTexture = TextureRegistry.LightningTrail2;
            NoiseTexture = TextureRegistry.LightningTrail3;
            PrimaryColor = Color.White;
            NoiseColor = Color.White;
            Speed = 10;
            Distortion = 0.2f;
            Power = 1.5f;
            BlendState = BlendState.Additive;
        }

        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["primaryTexture"].SetValue(PrimaryTexture.Value);
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["power"].SetValue(Power);
        }
    }
}

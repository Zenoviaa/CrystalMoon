using CrystalMoon.Registries;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class MagicPhantasmalShader : BaseShader
    {
        private static MagicPhantasmalShader _instance;
        public MagicPhantasmalShader()
        {
            Data = ShaderRegistry.MagicPhantasmalEffect;
            PrimaryTexture = TrailRegistry.StarTrail;
            NoiseTexture = TrailRegistry.StarTrail;
            Speed = 5;
            Repeats = 1;
        }

        public static MagicPhantasmalShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MagicPhantasmalShader();
                return _instance;
            }
        }

        public Asset<Texture2D> PrimaryTexture { get; set; }
        public Asset<Texture2D> NoiseTexture { get; set; }
        public float Speed { get; set; }
        public float Repeats { get; set; }
        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryTexture"].SetValue(PrimaryTexture.Value);
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["repeats"].SetValue(Repeats);
        }
    }
}

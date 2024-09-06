using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class SimpleTrailShader : BaseShader
    {
        private static SimpleTrailShader _instance;
        public SimpleTrailShader()
        {
            Data = ShaderRegistry.SimpleTrailEffect;
            TrailingTexture = TextureRegistry.StarTrail;
            SecondaryTrailingTexture = TextureRegistry.StarTrail;
            TertiaryTrailingTexture = TextureRegistry.StarTrail;
            PrimaryColor = Color.White;
            SecondaryColor = Color.White;
            Speed = 5;
        }

        public static SimpleTrailShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SimpleTrailShader();
                return _instance;
            }
        }

        public Asset<Texture2D> TrailingTexture { get; set; }
        public Asset<Texture2D> SecondaryTrailingTexture { get; set; }
        public Asset<Texture2D> TertiaryTrailingTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public float Speed { get; set; }

        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector4());
            Effect.Parameters["secondaryColor"].SetValue(SecondaryColor.ToVector4());
            Effect.Parameters["trailTexture"].SetValue(TrailingTexture.Value);
            Effect.Parameters["secondaryTrailTexture"].SetValue(SecondaryTrailingTexture.Value);
            Effect.Parameters["tertiaryTrailTexture"].SetValue(TertiaryTrailingTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}

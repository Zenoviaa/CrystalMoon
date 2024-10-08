﻿using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class MagicNaturalShader : BaseShader
    {
        private static MagicNaturalShader _instance;
        public MagicNaturalShader()
        {
            Data = ShaderRegistry.MagicTrailNatural;
            PrimaryTexture = TextureRegistry.NoiseTextureLeaves;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            ShapeTexture = TextureRegistry.DottedTrail;
            PrimaryColor = Color.White;
            NoiseColor = Color.White;
            Speed = 5;
            Distortion = 0.2f;
            Threshold = 0.3f;
        }

        public static MagicNaturalShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MagicNaturalShader();
                return _instance;
            }
        }

        public Asset<Texture2D> PrimaryTexture { get; set; }
        public Asset<Texture2D> NoiseTexture { get; set; }
        public Asset<Texture2D> ShapeTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color NoiseColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Threshold { get; set; }
        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["primaryTexture"].SetValue(PrimaryTexture.Value);
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["shapeTexture"].SetValue(ShapeTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["threshold"].SetValue(Threshold);
        }
    }
}

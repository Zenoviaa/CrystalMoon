﻿using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class FirePixelShader : BaseShader
    {
        private static FirePixelShader _instance;
        public FirePixelShader()
        {
            Data = ShaderRegistry.PixelMagicFire;
            PrimaryTexture = TextureRegistry.DottedTrail;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
   
            PrimaryColor = Color.White;
            NoiseColor = Color.White;

            Speed = 5;
            Distortion = 0.2f;
            Power = 1.5f;
        }

        public static FirePixelShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FirePixelShader();
                return _instance;
            }
        }

        public Asset<Texture2D> PrimaryTexture { get; set; }
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
            Data.UseColor(PrimaryColor);
            Data.UseSecondaryColor(NoiseColor);
            Data.UseImage1(PrimaryTexture);
            Data.UseImage2(NoiseTexture);
          
            Data.UseOpacity(Distortion);

   
            Effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["uIntensity"].SetValue(Power);
        }
    }
}

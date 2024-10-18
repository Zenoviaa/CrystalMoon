using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class MagicVaellusShader : BaseShader
    {
        private static MagicVaellusShader _instance;
        public MagicVaellusShader()
        {
            Data = ShaderRegistry.MagicTrailVaellus;
            PrimaryTexture = TextureRegistry.DottedTrail;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            OutlineTexture = TextureRegistry.DottedTrailOutline;
            PrimaryColor = Color.White;
            NoiseColor = Color.White;
            OutlineColor = Color.White;
            Speed = 5;
            Distortion = 0.2f;
            Power = 1.5f;
            Alpha = 1.0f;
        }

        public static MagicVaellusShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MagicVaellusShader();
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
        public float Alpha { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            PrimaryTexture = TextureRegistry.LightningTrail2;
            NoiseTexture = TextureRegistry.LightningTrail3;
            OutlineTexture = TextureRegistry.LightningTrail2Outline;
            PrimaryColor = new Color(69, 70, 159);
            NoiseColor = new Color(224, 107, 10);
            OutlineColor = Color.Lerp(new Color(31, 27, 59), Color.Black, 0.75f);
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.PointWrap;
            Speed = 5.2f;
            Distortion = 0.15f;
            Power = 0.25f;
            Alpha = 1f;
        }

        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["outlineColor"].SetValue(OutlineColor.ToVector3());
            Effect.Parameters["primaryTexture"].SetValue(PrimaryTexture.Value);
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["outlineTexture"].SetValue(OutlineTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["power"].SetValue(Power);
            Effect.Parameters["alpha"].SetValue(Alpha);
        }
    }

    internal class PixelMagicVaellusShader : BaseShader
    {
        private static PixelMagicVaellusShader _instance;
        public PixelMagicVaellusShader()
        {
            Data = ShaderRegistry.PixelMagicVaellus;
            PrimaryTexture = TextureRegistry.LightningTrail2;
            NoiseTexture = TextureRegistry.LightningTrail3;
            OutlineTexture = TextureRegistry.LightningTrail2Outline;
            PrimaryColor = new Color(69, 70, 159);
            NoiseColor = new Color(224, 107, 10);
            OutlineColor = Color.Lerp(new Color(31, 27, 59), Color.Black, 0.75f);
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.PointWrap;
            Speed = 5.2f;
            Distortion = 0.15f;
            Power = 0.25f;
            Blend = 0.4f;
        }

        public static PixelMagicVaellusShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PixelMagicVaellusShader();
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
        public float Blend { get; set; }
        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Effect.Parameters["outlineColor"].SetValue(OutlineColor.ToVector3());
            Effect.Parameters["primaryTexture"].SetValue(PrimaryTexture.Value);
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["outlineTexture"].SetValue(OutlineTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["power"].SetValue(Power);
            Effect.Parameters["blend"].SetValue(Blend);
        }
    }
}

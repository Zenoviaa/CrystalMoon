﻿using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class MagicCheckersShader : BaseShader
    {
        private static MagicCheckersShader _instance;
        public MagicCheckersShader()
        {
            Data = ShaderRegistry.MagicTrailCheckers;
            InnerTexture = TextureRegistry.CheckerTrail;
            TrailTexture = TextureRegistry.GlowTrailNoBlack;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            TrailOutlineTexture = TextureRegistry.GlowTrailNoBlackOutline;
            Speed = 5;
            Distortion = 0.2f;
            Power = 1.5f;
        }

        public static MagicCheckersShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MagicCheckersShader();
                return _instance;
            }
        }

        public Asset<Texture2D> InnerTexture { get; set; }
        public Asset<Texture2D> TrailTexture { get; set; }
        public Asset<Texture2D> NoiseTexture { get; set; }
        public Asset<Texture2D> TrailOutlineTexture { get; set; }
        public Color OutlineColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Power { get; set; }
        public float Threshold { get; set; }
        public float Tightness { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            InnerTexture = TextureRegistry.CheckerTrail;
            TrailTexture = TextureRegistry.GlowTrailNoBlack;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            TrailOutlineTexture = TextureRegistry.GlowTrailNoBlackOutline;
            OutlineColor = Color.Black;
            Speed = 5;
            Distortion = 0.2f;
            Power = 1.5f;
            Threshold = 0.75f;
            Tightness = 5f;
        }

        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["threshold"].SetValue(Threshold);
            Effect.Parameters["innerTexture"].SetValue(InnerTexture.Value);
            Effect.Parameters["trailTexture"].SetValue(TrailTexture.Value);
            Effect.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Effect.Parameters["trailOutlineTexture"].SetValue(TrailOutlineTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Effect.Parameters["distortion"].SetValue(Distortion);
            Effect.Parameters["power"].SetValue(Power);
            Effect.Parameters["tightness"].SetValue(Tightness);
            Effect.Parameters["outlineColor"].SetValue(OutlineColor.ToVector3());
        }
    }
}

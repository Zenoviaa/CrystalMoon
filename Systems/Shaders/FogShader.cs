﻿using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CrystalMoon.Systems.Shaders
{
    internal class FogShader : BaseShader
    {
        private static FogShader _instance;
        public static FogShader Instance
        {
            get
            {
                _instance ??= new();
                _instance.SetDefaults();
                return _instance;
            }
        }

        public Asset<Texture2D> FogTexture { get; set; }
        public float Speed { get; set; }
        public float EdgePower { get; set; }
        public float ProgressPower { get; set; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            FogTexture = TextureRegistry.NoiseTextureClouds3;
            Speed = 0.5f;
            EdgePower = 1.5f;
            ProgressPower = 2.5f;
        }

        protected override void OnApply()
        {
            base.OnApply();
           // Effect.Parameters["fogTexture"].SetValue(FogTexture.Value);
            Effect.Parameters["edgePower"].SetValue(EdgePower);
            Effect.Parameters["progressPower"].SetValue(ProgressPower);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}

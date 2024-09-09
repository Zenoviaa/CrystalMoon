using CrystalMoon.Registries;
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
                if (_instance == null)
                    _instance = new FogShader();
                return _instance;
            }
        }

        public FogShader()
        {
            Data = ShaderRegistry.FogShader;
            Speed = 0.5f;
            EdgePower = 1.5f;
            ProgressPower = 2.5f;
        }


        public Asset<Texture2D> FogTexture
        {
            set => Data.UseImage1(value);
        }

        public float Speed { get; set; }
        public float EdgePower
        {
            get => Effect.Parameters["edgePower"].GetValueSingle();
            set => Effect.Parameters["edgePower"].SetValue(value);
        }

        public float ProgressPower
        {
            get => Effect.Parameters["progressPower"].GetValueSingle();
            set => Effect.Parameters["progressPower"].SetValue(value);
        }

        protected override void OnApply()
        {
            base.OnApply();
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}

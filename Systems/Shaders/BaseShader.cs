using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace CrystalMoon.Systems.Shaders
{
    public abstract class BaseShader
    {
        public MiscShaderData Data { get; protected set; }
        public Effect Effect => Data.Shader;
        public BlendState BlendState { get; set; } = BlendState.Additive;
        public SamplerState SamplerState { get; set; } = SamplerState.LinearWrap;
        public bool FillShape { get; set; }
        public int DrawCount { get; set; } = 1;
        public virtual void Apply() { OnApply(); }
        protected virtual void OnApply()
        {

        }
        public virtual void ApplyShaderData()
        {
            OnApply();
            Data.Apply();
        }

        public virtual void SetDefaults()
        {

        }

        public virtual void ResetDefaults()
        {
            FillShape = false;
            DrawCount = 1;
        }
    }
}

using Microsoft.Xna.Framework.Graphics;

namespace CrystalMoon.Systems.Shaders
{
    public abstract class BaseShader
    {
        public Effect Effect { get; protected set; }
        public BlendState BlendState { get; set; } = BlendState.Additive;
        public SamplerState SamplerState { get; set; } = SamplerState.LinearWrap;
        public bool FillShape { get; set; }
        public int DrawCount { get; set; } = 1;
        public abstract void Apply();
        public virtual void ResetDefaults()
        {
            FillShape = false;
            DrawCount = 1;
        }
    }
}

using CrystalMoon.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework;

namespace CrystalMoon.Systems
{
    internal class Fog
    {
        public Point tilePosition;
        public Vector2 position;
        public Vector2 scale;
        public Vector2 startScale;
        public Color color;
        public Color startColor;
        public void Update()
        {
            float p = MathUtil.Osc(0f, 1f, speed: 1.0f, offset: position.X + position.Y);
            float ep = Easing.SpikeOutCirc(p);
            color = Color.Lerp(startColor * 0.95f, startColor, ep);
            scale = Vector2.Lerp(startScale * 0.95f, startScale, ep);
        }
    }
}

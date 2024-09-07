using System;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseElement : BaseMagicItem,
        ICloneable,
        IAdvancedMagicAddon
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public virtual void AI() { }
        public virtual void DrawTrail() { }
        public virtual void ApplyFormShader() { }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public BaseElement Instantiate()
        {
            return (BaseElement)Clone();
        }
    }
}

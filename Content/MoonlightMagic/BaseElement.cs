using System;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseElement : ModItem,
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

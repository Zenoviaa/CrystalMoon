using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseElement : IAdvancedMagicAddon
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        public virtual void AI() { }
        public virtual void DrawTrail() { }
        public virtual void ApplyFormShader() { }
    }
}

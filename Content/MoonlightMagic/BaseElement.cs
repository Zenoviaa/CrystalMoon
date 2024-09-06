using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseElement : IMagicAddon
    {
        public MoonlightMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        public virtual void AI() { }
        public virtual void DrawTrail() { }
    }
}

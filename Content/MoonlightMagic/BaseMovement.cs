using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseMovement : IMagicAddon
    {
        public MoonlightMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        public abstract void AI();
    }
}

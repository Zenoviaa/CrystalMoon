using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal interface IMagicAddon
    {
        public MoonlightMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
    }
}

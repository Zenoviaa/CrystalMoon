using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal interface IAdvancedMagicAddon
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
    }
}

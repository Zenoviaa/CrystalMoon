using Terraria;

namespace CrystalMoon.Content.Bases.MoonlightMagic
{
    internal abstract class BaseEnchantment
    {
        public virtual void SetDefaults(MoonlightMagicProjectile proj) { }
        public virtual void AI(MoonlightMagicProjectile proj) { }
        public virtual void OnHitNPC(MoonlightMagicProjectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnKill(MoonlightMagicProjectile proj, int timeLeft) { }
    }
}

using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseEnchantmentItem<T> : ModItem 
        where T : BaseEnchantment, new()
    {
        public T Enchant => new();


        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }

    internal abstract class BaseEnchantment :
        IAdvancedMagicAddon
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        //Enchantment stuff
        public virtual void SetDefaults() { }
        public virtual void AI() { }
        public virtual void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnKill(int timeLeft) { }
    }
}

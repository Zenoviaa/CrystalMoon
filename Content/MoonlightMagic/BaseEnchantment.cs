using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseEnchantment : BaseMagicItem, 
        IAdvancedMagicAddon,
        ICloneable
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        public object Clone()
        {
            return MemberwiseClone();
        }

        public BaseEnchantment Instantiate()
        {
            return (BaseEnchantment)Clone(); 
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }


        //Enchantment stuff
        public virtual void AI() { }
        public virtual void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnKill(int timeLeft) { }
    }
}

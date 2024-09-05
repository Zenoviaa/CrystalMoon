using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Bases.MoonlightMagic
{
    internal class MoonlightMagicProjectile : ModProjectile
    {
        public override string Texture => TextureRegistry.EmptyTexturePath;
        public List<BaseEnchantment> Enchantments { get; private set; } = new List<BaseEnchantment>();
        public BaseElement PrimaryElement { get; private set; }
        public BaseForm Form { get; private set; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.timeLeft = 360;
            for (int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.SetDefaults(this);
            }
        }

        public override void AI()
        {
            base.AI();
            for (int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.AI(this);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            for(int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.OnHitNPC(this, target, hit, damageDone);
            }
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            for (int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.OnKill(this, timeLeft);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
        }
    }
}

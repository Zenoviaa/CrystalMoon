using CrystalMoon.Content.MoonlightMagic.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Guuts
{
    internal class HeavyMetalEnchantment : BaseEnchantment
    {
        private int _timer;
        public override void SetMagicDefaults()
        {
            base.SetMagicDefaults();
            float damage = Projectile.damage;
            damage *= 1.12f;
            Projectile.damage = (int)damage;
        }
        public override void AI()
        {
            base.AI();

            //Count up
            _timer++;
            if (_timer == 1 && Main.myPlayer == Projectile.owner)
            {
                MagicProj.OldPos[0] = Vector2.Zero;
                Vector2 center = Main.MouseWorld;

                Player player = Main.player[Projectile.owner];
                float off = 1000;
                Vector2 spawnPos = new Vector2(center.X, player.Center.Y - off);
                Projectile.Center = spawnPos - Vector2.UnitY * (MagicProj.Size + 2);
                Projectile.velocity = Vector2.UnitY * Projectile.velocity.Length();
                Projectile.netUpdate = true;
            }
        }

        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<BloodletElement>();
        }

    }
}

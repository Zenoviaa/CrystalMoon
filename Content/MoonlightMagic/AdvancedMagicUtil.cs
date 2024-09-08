using CrystalMoon.Systems.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal static class AdvancedMagicUtil
    {
        public static void NewMagicProjectile(BaseStaff item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 0;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile staff = Projectile.NewProjectileDirect(
                source, position, velocity, ModContent.ProjectileType<AdvancedMagicStaffProjectile>(), damage, knockback, player.whoAmI, 
                ai0: 0, ai1: dir, ai2: item.Item.useTime);
            comboPlayer.IncreaseCombo(maxCombo: 0);
            
            AdvancedMagicStaffProjectile proj = staff.ModProjectile as AdvancedMagicStaffProjectile;
            proj.staffSource = item;
        }

        public static void NewMagicProjectile(BaseStaff item, Projectile sourceProjectile)
        {
            Projectile p = Projectile.NewProjectileDirect(
                                sourceProjectile.GetSource_FromThis(), sourceProjectile.Center, sourceProjectile.velocity,
                                ModContent.ProjectileType<AdvancedMagicProjectile>(), sourceProjectile.damage, sourceProjectile.knockBack, sourceProjectile.owner);
            //Set Moonlight Defaults
            AdvancedMagicProjectile moonlightMagicProjectile = p.ModProjectile as AdvancedMagicProjectile;
            moonlightMagicProjectile.TrailLength = item.TrailLength;
            moonlightMagicProjectile.Size = item.Size;
            moonlightMagicProjectile.SetMoonlightDefaults(item);
        }
    }
}

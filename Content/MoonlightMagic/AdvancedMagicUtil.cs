using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal static class AdvancedMagicUtil
    {
        public static void NewMagicProjectile(BaseStaff item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);

            //Set Moonlight Defaults
            AdvancedMagicProjectile moonlightMagicProjectile = p.ModProjectile as AdvancedMagicProjectile;
            moonlightMagicProjectile.TrailLength = item.TrailLength;
            moonlightMagicProjectile.SetMoonlightDefaults(item);
        }
    }
}

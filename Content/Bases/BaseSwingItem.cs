using CrystalMoon.Systems.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CrystalMoon.Content.Bases
{
    public abstract class BaseSwingItem : ModItem
    {
        public int comboWaitTime = 60;
        public int maxCombo;
        public int maxStaminaCombo;
        public int staminaProjectileShoot;
        public int staminaToUse;
        public virtual void ShootSwing(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = comboWaitTime;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);
            comboPlayer.IncreaseCombo(maxCombo: maxCombo);
        }

        public virtual void ShootSwingStamina(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = comboWaitTime;
            comboPlayer.ConsumeStamina(staminaToUse);
        
            int combo = comboPlayer.StaminaComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);
            comboPlayer.IncreaseStaminaCombo(maxStaminaCombo: maxStaminaCombo);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            if (player.altFunctionUse == 2 && comboPlayer.CanUseStamina(staminaToUse))
            {
                ShootSwingStamina(player, source, position, velocity, staminaProjectileShoot, damage, knockback);
            }
            else
            {
                ShootSwing(player, source, position, velocity, type, damage, knockback);
            }
            
            return false;
        }
    }
}

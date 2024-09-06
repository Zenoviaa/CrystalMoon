using CrystalMoon.ExampleContent.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.ExampleContent.Items
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class Fake : ModItem
    {
        private int _index;
        private static List<int> _projectilesToShoot = new();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            _projectilesToShoot = new();
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleTrailingProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleCircleExplosionProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicHexProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicRadianceProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicBloodletProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicSparkleWaterProjectile>());
        }

        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CrystalMoon.hjson' file.
        public override void SetDefaults()
        {
    
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ExampleMoonlightMagicRadianceProjectile>();
            Item.shootSpeed = 15;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            int typeToShoot = _projectilesToShoot[_index];
            string name = ModContent.GetModProjectile(typeToShoot).Name;
            TooltipLine firing = new TooltipLine(Mod, "Helper", $"Shooting: {name}");
            tooltips.Add(firing);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                _index++;
                if (_index >= _projectilesToShoot.Count)
                {
                    _index = 0;
                }
                int typeToShoot = _projectilesToShoot[_index];
                string name = ModContent.GetModProjectile(typeToShoot).Name;
                CombatText.NewText(player.getRect(), Color.White, name, true);
            }
      
            Projectile.NewProjectile(source, position, velocity, _projectilesToShoot[_index], damage, knockback, player.whoAmI);
            return false;
        }
    }
}

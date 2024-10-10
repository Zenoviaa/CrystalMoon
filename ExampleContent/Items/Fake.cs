using CrystalMoon.ExampleContent.Projectiles;
using CrystalMoon.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicDeeyaProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicPhantasmalProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicGuutProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicNaturalProjectile>());
            _projectilesToShoot.Add(ModContent.ProjectileType<ExampleMoonlightMagicNormalProjectile>());
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

        private void CameraModifierExamples()
        {

            /*
             startPosition is the starting position of the screen shake
             direction is which way it is going to move
             strength is how far it moves
             vibrationCyclesPerSecond is how many times it shakes in a single second, the higher, the more erratic
             frames is how many ticks it lasts
             */


            //Passing in velocity to the direction variable is cool for like recoil effects
            CrystalMoonFXUtil.PunchCamera(
                startPosition: Main.LocalPlayer.position,
                direction: Vector2.UnitY * 8,
                strength: 4,
                vibrationCyclesPerSecond: 2,
                frames: 30,
                distanceFallOff: -1);

            //The normal screenshake we all know, variables are self-explanatory
            //Distance is how much falloff the shake has based on how far you are from it
            CrystalMoonFXUtil.ShakeCamera(position: Main.LocalPlayer.position, distance: 1024, strength: 80);

            //Camera focus system
            //You can focus NPCs/Players/Entities directly like this
            CrystalMoonFXUtil.FocusCamera(Main.LocalPlayer, duration: 120);

            //You can also focus a specific position
            CrystalMoonFXUtil.FocusCamera(Vector2.Zero, duration: 120);
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

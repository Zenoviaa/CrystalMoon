using CrystalMoon.Content.MoonlightMagic;
using CrystalMoon.ExampleContent.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.ExampleContent.Items
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class EnchantmentKiller : ModItem
    {
        private int _index;
      
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


        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.GetModPlayer<AdvancedMagicPlayer>().Backpack.Clear();
            return true;
        }
    }
}


using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Tiles.MothlightTiles
{
    public class MothlightGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            RegisterItemDrop(ModContent.ItemType<MothlightGrassBlock>());
           // DustType = Main.rand.Next(110, 113);

            MineResist = 1f;
            MinPick = 25;

            AddMapEntry(new Color(10, 74, 101));

            // TODO: implement
            // SetModTree(new Trees.ExampleTree());
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override bool CanExplode(int i, int j) => false;
        // TODO: implement
        // public override void ChangeWaterfallStyle(ref int style) {
        // 	style = mod.GetWaterfallStyleSlot("ExampleWaterfallStyle");
        // }
        //
        // public override int SaplingGrowthType(ref int style) {
        // 	style = 0;
        // 	return TileType<ExampleSapling>();
        // }
    }








    public class MothlightGrassBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Super silk!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<MothlightGrass>();
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon
{
    internal class VanillaTextureReplaceSystem : ModSystem
    {
        public override void Load()
        {
            base.Load();
            var config = ModContent.GetInstance<CrystalMoonClientConfig>();
  

            if (!Main.dedServ && Main.netMode != NetmodeID.Server && config.VanillaRespritesToggle == true)
            {
                Main.instance.LoadTiles(TileID.WoodBlock);
                TextureAssets.Tile[TileID.WoodBlock] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/RefinedWoodTile");

                Main.instance.LoadTiles(TileID.Dirt);
                TextureAssets.Tile[TileID.Dirt] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/CourseDirt");

                Main.instance.LoadTiles(TileID.Grass);
                TextureAssets.Tile[TileID.Grass] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/CourseGrass");

                Main.instance.LoadTiles(TileID.ClayBlock);
                TextureAssets.Tile[TileID.ClayBlock] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/ClayTile");

                Main.instance.LoadTiles(TileID.GrayBrick);
                TextureAssets.Tile[TileID.GrayBrick] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/StoneBrick");

                Main.instance.LoadTiles(TileID.Stone);
                TextureAssets.Tile[TileID.Stone] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/StoneREDO");

                //Set tree tops
                //Number inside is the one you're replacing, up to 31


                //Forest
                Main.instance.LoadTiles(TileID.Trees);
                TextureAssets.Tile[TileID.Trees] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeWood");

                TextureAssets.TreeTop[0] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeNormal1");
                TextureAssets.TreeTop[6] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeNormal1");
                TextureAssets.TreeTop[7] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeNormal2");
                TextureAssets.TreeTop[8] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeNormal2");
                TextureAssets.TreeTop[9] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeNormal1");
                TextureAssets.TreeTop[10] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/TreeNormal3");

                TextureAssets.TreeBranch[0] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/NormalTreeBranches");
                TextureAssets.TreeBranch[6] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/NormalTreeBranches");
                TextureAssets.TreeBranch[7] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/NormalTreeBranches");
                TextureAssets.TreeBranch[8] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/NormalTreeBranches");
                TextureAssets.TreeBranch[9] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/NormalTreeBranches");
                TextureAssets.TreeBranch[10] = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/TextureRemake/NormalTreeBranches");
                //---------------------------------------------------

            }


            if (!Main.dedServ && Main.netMode != NetmodeID.Server && config.VanillaUIRespritesToggle)
            {
                //Replace UI
                string categoryPanel = "CrystalMoon/Assets/Textures/UI/CategoryPanel";
                string categoryPanelHot = "CrystalMoon/Assets/Textures/UI/CategoryPanelHot";
             
                TextureAssets.InventoryBack = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack2 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack3 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack4 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack5 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack6 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack7 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack8 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack9 = ModContent.Request<Texture2D>(categoryPanel);
                TextureAssets.InventoryBack10 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack11 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack12 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack13 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack14 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack15 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack16 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack17 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack18 = ModContent.Request<Texture2D>(categoryPanelHot);
                TextureAssets.InventoryBack19 = ModContent.Request<Texture2D>(categoryPanelHot);


                TextureAssets.ScrollLeftButton = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/BackButton");
                TextureAssets.ScrollRightButton = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/ForwardButton");
            }
        }

        public override void Unload()
        {
            base.Unload();
            //Unload UI Resprites
            if (!Main.dedServ)
            {
                string backButton = "Terraria/Images/UI/Bestiary/Button_Back";
                string forwardButton = "Terraria/Images/UI/Bestiary/Button_Forward";

                TextureAssets.InventoryBack = ModContent.Request<Texture2D>(InventoryBackPath(0));
                TextureAssets.InventoryBack2 = ModContent.Request<Texture2D>(InventoryBackPath(2));
                TextureAssets.InventoryBack3 = ModContent.Request<Texture2D>(InventoryBackPath(3));
                TextureAssets.InventoryBack4 = ModContent.Request<Texture2D>(InventoryBackPath(4));
                TextureAssets.InventoryBack5 = ModContent.Request<Texture2D>(InventoryBackPath(5));
                TextureAssets.InventoryBack6 = ModContent.Request<Texture2D>(InventoryBackPath(6));
                TextureAssets.InventoryBack7 = ModContent.Request<Texture2D>(InventoryBackPath(7));
                TextureAssets.InventoryBack8 = ModContent.Request<Texture2D>(InventoryBackPath(8));
                TextureAssets.InventoryBack9 = ModContent.Request<Texture2D>(InventoryBackPath(9));
                TextureAssets.InventoryBack10 = ModContent.Request<Texture2D>(InventoryBackPath(10));
                TextureAssets.InventoryBack11 = ModContent.Request<Texture2D>(InventoryBackPath(11));
                TextureAssets.InventoryBack12 = ModContent.Request<Texture2D>(InventoryBackPath(12));
                TextureAssets.InventoryBack13 = ModContent.Request<Texture2D>(InventoryBackPath(13));
                TextureAssets.InventoryBack14 = ModContent.Request<Texture2D>(InventoryBackPath(14));
                TextureAssets.InventoryBack15 = ModContent.Request<Texture2D>(InventoryBackPath(15));
                TextureAssets.InventoryBack16 = ModContent.Request<Texture2D>(InventoryBackPath(16));
                TextureAssets.InventoryBack17 = ModContent.Request<Texture2D>(InventoryBackPath(17));
                TextureAssets.InventoryBack18 = ModContent.Request<Texture2D>(InventoryBackPath(18));
                TextureAssets.InventoryBack19 = ModContent.Request<Texture2D>(InventoryBackPath(19));
                TextureAssets.ScrollLeftButton = ModContent.Request<Texture2D>(backButton);
                TextureAssets.ScrollRightButton = ModContent.Request<Texture2D>(forwardButton);
            }

            if (!Main.dedServ)
            {
                //Unload Tree Tops
                UnloadTreeTops(0);
                UnloadTreeTops(6);
                UnloadTreeTops(7);
                UnloadTreeTops(8);
                UnloadTreeTops(9);
                UnloadTreeTops(10);

                //Unload Tree Branches
                UnloadTreeBranch(0);
                UnloadTreeBranch(6);
                UnloadTreeBranch(7);
                UnloadTreeBranch(8);
                UnloadTreeBranch(9);
                UnloadTreeBranch(10);

                //Unload Tiles
                UnloadTile(TileID.Grass);
                UnloadTile(TileID.Dirt);
                UnloadTile(TileID.ClayBlock);
                UnloadTile(TileID.WoodBlock);
                UnloadTile(TileID.GrayBrick);
                UnloadTile(TileID.Stone);
                UnloadTile(TileID.Trees);
            }
        }

        private string InventoryBackPath(int tileID)
        {
            if(tileID == 0)
                return $"Terraria/Images/Inventory_Back";
            return $"Terraria/Images/Inventory_Back{tileID}";
        }

        private void UnloadTreeTops(int tileID)
        {
            TextureAssets.TreeTop[tileID] = ModContent.Request<Texture2D>($"Terraria/Images/Tree_Tops_{tileID}");
        }
        private void UnloadTreeBranch(int tileID)
        {
            TextureAssets.TreeBranch[tileID] = ModContent.Request<Texture2D>($"Terraria/Images/Tree_Branches_{tileID}");
        }

        private void UnloadTile(int tileID)
        {
            TextureAssets.Tile[tileID] = ModContent.Request<Texture2D>($"Terraria/Images/Tiles_{tileID}");
        }

        private void UnloadWall(int wallID)
        {
            TextureAssets.Wall[wallID] = ModContent.Request<Texture2D>($"Terraria/Images/Wall_{wallID}");
        }
    }
}

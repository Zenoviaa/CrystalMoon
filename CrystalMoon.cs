using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using System.IO;
using System.Reflection;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.UI;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria.GameContent.UI.States;
using Terraria.GameContent.Creative;


namespace CrystalMoon
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CrystalMoon : Mod
    {
        public const string EMPTY_TEXTURE = "CrystalMoon/Assets/Empty";
        public static Texture2D EmptyTexture
        {
            get;
            private set;
        }
        public int GlobalTimer { get; private set; }

        public Asset<Effect> Whiteout;



        // this is alright, and i'll expand it so it can still be used, but really this shouldn't be used
        public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);

            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                if (obj is bool) packet.Write((bool)obj);
                else if (obj is byte) packet.Write((byte)obj);
                else if (obj is int) packet.Write((int)obj);
                else if (obj is float) packet.Write((float)obj);
                else if (obj is double) packet.Write((double)obj);
                else if (obj is short) packet.Write((short)obj);
                else if (obj is ushort) packet.Write((ushort)obj);
                else if (obj is sbyte) packet.Write((sbyte)obj);
                else if (obj is uint) packet.Write((uint)obj);
                else if (obj is decimal) packet.Write((decimal)obj);
                else if (obj is long) packet.Write((long)obj);
                else if (obj is string) packet.Write((string)obj);
            }
            return packet;
        }


        public static Player PlayerExists(int whoAmI)
        {
            return whoAmI > -1 && whoAmI < Main.maxPlayers && Main.player[whoAmI].active && !Main.player[whoAmI].dead && !Main.player[whoAmI].ghost ? Main.player[whoAmI] : null;
        }


        public CrystalMoon()
        {
            Instance = this;
        }

        public static CrystalMoon Instance;
        public override void Load()
        {
            /*
            CrystalMoonUtils.LoadShaders();
            CrystalMoonUtils.LoadOrderedLoadables();
            Instance = this;
            Whiteout = Assets.Request<Effect>("Assets/Effects/Whiteout");
            Ref<Effect> GenericLaserShader = new(Assets.Request<Effect>("Assets/Effects/LaserShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["CrystalMoon:LaserShader"] = new MiscShaderData(GenericLaserShader, "TrailPass");
            */
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            //``````````````````````````````````````````````````````````````````````````````````````


            //`````````````````````````````````````````````````````````````````````````````





            if (!Main.dedServ && Main.netMode != NetmodeID.Server && ModContent.GetInstance<CrystalMoonClientConfig>().VanillaRespritesToggle == true)
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







                ///---------- UI Panels
                ///



            }

            if (!Main.dedServ && Main.netMode != NetmodeID.Server && ModContent.GetInstance<CrystalMoonClientConfig>().VanillaUIRespritesToggle == true)
            {
                TextureAssets.InventoryBack = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack2 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack3 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack4 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack5 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack6 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack7 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack8 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack9 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanel");
                TextureAssets.InventoryBack10 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack11 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack12 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack13 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack14 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack15 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack16 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack17 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack18 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");
                TextureAssets.InventoryBack19 = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/CategoryPanelHot");

                TextureAssets.ScrollLeftButton = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/BackButton");
                TextureAssets.ScrollRightButton = ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/ForwardButton");

                On_UIWorldListItem.DrawSelf += DrawWorldSelecItemOverlayUI;
                On_UIWorldCreationPreview.DrawSelf += DrawWorldPreviewUI;
            }

            Instance = this;
        }

        private void DrawWorldPreviewUI(On_UIWorldCreationPreview.orig_DrawSelf orig, UIWorldCreationPreview self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            DrawWorldPreview(self, spriteBatch);
        }

        private void DrawWorldSelecItemOverlayUI(On_UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            DrawWorldSelectItemOverlay(self, spriteBatch);
        }

        private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
        {
            UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
            Asset<Texture2D> WorldPanel = (Asset<Texture2D>)typeof(UIWorldListItem).GetField("_innerPanelTexture", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);

            WorldIcon.RemoveAllChildren();


            Asset<Texture2D> worldPanelBG = WorldPanel;

            UIImage element2 = new UIImage(ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/InnerPanelBackgroundNew"))
            {
                Top = new StyleDimension(-0f, 0f),
                Left = new StyleDimension(-0f, 0f),
                IgnoresMouseInteraction = true
            };


            UIElement worldIcon = WorldIcon;

            UIImage element = new UIImage(ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/Menu/LunarTree"))
            {
                Top = new StyleDimension(-10f, 0f),
                Left = new StyleDimension(-6f, 0f),
                IgnoresMouseInteraction = true
            };





            worldIcon.Append(element);


        }

        private void DrawWorldPreview(UIWorldCreationPreview uiItem, SpriteBatch spriteBatch)
        {
            typeof(UIWorldCreationPreview).GetField("_SizeSmallTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewSizeWorldVeil"));
            typeof(UIWorldCreationPreview).GetField("_SizeMediumTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewSizeWorldVeil"));
            typeof(UIWorldCreationPreview).GetField("_SizeLargeTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewSizeWorldVeil"));

            typeof(UIWorldCreationPreview).GetField("_EvilRandomTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewBlankWorld"));
            typeof(UIWorldCreationPreview).GetField("_EvilCorruptionTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewBlankWorld"));
            typeof(UIWorldCreationPreview).GetField("_EvilCrimsonTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewBlankWorld"));


            typeof(UIWorldCreationPreview).GetField("_BunnyMasterTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewDifficultyMaster"));
            typeof(UIWorldCreationPreview).GetField("_BunnyExpertTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewDifficultyExpert"));
            typeof(UIWorldCreationPreview).GetField("_BunnyNormalTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewDifficultyNormal"));
            typeof(UIWorldCreationPreview).GetField("_BunnyCreativeTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewDifficultyMJourny"));

            typeof(UIWorldCreationPreview).GetField("_BorderTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(uiItem, ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/PreviewBorder"));
        }

        public override void Unload()
        {
            On_UIWorldListItem.DrawSelf -= DrawWorldSelecItemOverlayUI;
            On_UIWorldCreationPreview.DrawSelf -= DrawWorldPreviewUI;
          //  CrystalMoonUtils.UnloadOrderedLoadables();

            Whiteout = null;


            if (!Main.dedServ)
            {
                UnloadTile(TileID.Grass);
                UnloadTile(TileID.Dirt);
                UnloadTile(TileID.WoodBlock);
                UnloadTile(TileID.GrayBrick);
                UnloadTile(TileID.Stone);
            }
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
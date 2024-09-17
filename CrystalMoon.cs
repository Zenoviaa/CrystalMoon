using CrystalMoon.Registries;
using CrystalMoon.Systems;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


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
            ShaderRegistry.LoadShaders();
            CrystalMoonUtils.LoadShaders();
            CrystalMoonUtils.LoadOrderedLoadables();

            if (!Main.dedServ && Main.netMode != NetmodeID.Server && ModContent.GetInstance<CrystalMoonClientConfig>().VanillaUIRespritesToggle == true)
            {
                On_UIWorldListItem.DrawSelf += DrawWorldSelecItemOverlayUI;
            }

            Instance = this;
        }


        private void DrawWorldSelecItemOverlayUI(On_UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            DrawWorldSelectItemOverlay(self, spriteBatch);
        }

        private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
        {
            UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
            WorldIcon.RemoveAllChildren();

            UIElement worldIcon = WorldIcon;
            UIImage element = new UIImage(ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/Menu/LunarTree"))
            {
                Top = new StyleDimension(-10f, 0f),
                Left = new StyleDimension(-6f, 0f),
                IgnoresMouseInteraction = true
            };

            worldIcon.Append(element);
        }

        public override void Unload()
        {
            On_UIWorldListItem.DrawSelf -= DrawWorldSelecItemOverlayUI;
            Whiteout = null;
        }
    }
}
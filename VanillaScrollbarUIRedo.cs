using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace CrystalMoon
{
    internal class VanillaScrollbarUIRedo : ModSystem
    {
        public override void Load()
        {
            base.Load();
            On_UIScrollbar.DrawSelf += DrawScrollbarUI;
        }

        public override void Unload()
        {
            base.Unload();

            On_UIScrollbar.DrawSelf -= DrawScrollbarUI;
        }

        private void DrawScrollbarUI(On_UIScrollbar.orig_DrawSelf orig, UIScrollbar self, SpriteBatch spriteBatch)
        {
            //Just call the normal func
            if (!ModContent.GetInstance<CrystalMoonClientConfig>().VanillaUIRespritesToggle)
            {
                orig(self, spriteBatch);
                return;
            }

            //Replace sprites
            typeof(UIScrollbar).GetField("_texture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, 
                ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/ScrollBarOuter"));
            typeof(UIScrollbar).GetField("_innerTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self,
                ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/UI/ScrollBarIn"));

            orig(self, spriteBatch);

            //Set back to normal so there's no error when unloading the mod
            typeof(UIScrollbar).GetField("_texture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, Main.Assets.Request<Texture2D>("Images/UI/Scrollbar"));
            typeof(UIScrollbar).GetField("_innerTexture", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner"));
        }
    }
}

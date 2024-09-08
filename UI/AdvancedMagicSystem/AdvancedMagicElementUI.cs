using CrystalMoon.Content.MoonlightMagic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicElementUI : UIPanel
    {
        private BaseStaff ActiveStaff => AdvancedMagicUISystem.Staff;
        internal const int width = 480;
        internal const int height = 155;

        internal int RelativeLeft => Main.screenWidth - 614;
        internal int RelativeTop => Main.screenHeight / 2 - 314;

        public AdvancedMagicElementSlot ElementSlot { get; private set; }
        public AdvancedMagicElementUI() : base()
        {

        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            Width.Pixels = width;
            Height.Pixels = height;
            Left.Pixels = RelativeLeft;
            Top.Pixels = RelativeTop;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;
            ElementSlot = new();
            Append(ElementSlot);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Constantly lock the UI in the position regardless of resolution changes
            Left.Pixels = RelativeLeft;
            Top.Pixels = RelativeTop;
        }
    }
}

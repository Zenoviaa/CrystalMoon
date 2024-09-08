using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicButtonUI : UIPanel
    {
        private UIImage _btn;

        internal const int width = 100;
        internal const int height = 100;

        internal int RelativeLeft => 505;
        internal int RelativeTop => Main.screenHeight / 2 - 96;

        public override void OnInitialize()
        {
            base.OnInitialize();

            Width.Pixels = width;
            Height.Pixels = height;
            Top.Pixels = int.MaxValue / 2;
            Left.Pixels = int.MaxValue / 2;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;
            _btn = new UIImage(ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/Paper"));
            _btn.OnLeftClick += (evt, element) => { ToggleUI(); };
            _btn.Activate();
            Append(_btn);
        }

        private void ToggleUI()
        {
            AdvancedMagicUISystem.ToggleUI();
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

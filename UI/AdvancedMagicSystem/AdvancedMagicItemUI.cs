using CrystalMoon.Content.MoonlightMagic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicItemUI : UIPanel
    {
        private UIGrid _enchantmentsSlotGrid;

        internal const int width = 480;
        internal const int height = 155;

        internal int RelativeLeft => 470 + 108;
        internal int RelativeTop => 0 + 12;

        public override void OnInitialize()
        {
            base.OnInitialize();
            Width.Pixels = 48*4f;
            Height.Pixels = 48*16;
            Left.Pixels = RelativeLeft;
            Top.Pixels = RelativeTop;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;

            _enchantmentsSlotGrid = new UIGrid();
            _enchantmentsSlotGrid.Width.Set(0, 1f);
            _enchantmentsSlotGrid.Height.Set(0, 1f);
            _enchantmentsSlotGrid.HAlign = 0.5f;
            _enchantmentsSlotGrid.ListPadding = 2f;
            Append(_enchantmentsSlotGrid);
        }

        public override void Recalculate()
        {
            //Recalculate the UI when there is some sort of update
            Left.Pixels = RelativeLeft;
            Top.Pixels = RelativeTop;
            _enchantmentsSlotGrid?.Clear();
            if (Main.gameMenu)
                return;

            var magicPlayer = Main.LocalPlayer.GetModPlayer<AdvancedMagicPlayer>();
            for (int i = 0; i < magicPlayer.Backpack.Count; i++)
            {
                AdvancedMagicItemSlot slot = new AdvancedMagicItemSlot();
                slot.index = i;
                slot.Item = magicPlayer.Backpack[i].Clone();

                _enchantmentsSlotGrid.Add(slot);
            }

            _enchantmentsSlotGrid.Recalculate();
            base.Recalculate();
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

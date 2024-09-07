using CrystalMoon.Content.MoonlightMagic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicItemUI : UIPanel
    { 
        internal const int width = 480;
        internal const int height = 155;

        internal int RelativeLeft => 470 + 108;
        internal int RelativeTop => 0 + 12;

        internal UIGrid enchantmentsGrid;

        public List<AdvancedMagicItemSlot> ItemSlots { get; private set; } = new();
        public override void OnInitialize()
        {
            base.OnInitialize();
            Width.Pixels = 48*4f;
            Height.Pixels = 48*16;
            Top.Pixels = int.MaxValue / 2;
            Left.Pixels = int.MaxValue / 2;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;
            enchantmentsGrid = new UIGrid();
            enchantmentsGrid.Width.Set(0, 1f);
            enchantmentsGrid.Height.Set(0, 1f);
            enchantmentsGrid.HAlign = 0.5f;
            enchantmentsGrid.ListPadding = 2f;
            Append(enchantmentsGrid);
            SetupSlots();
        }

        private void SetupSlots()
        {
            var magicPlayer = Main.LocalPlayer.GetModPlayer<AdvancedMagicPlayer>();
            for(int i = 0; i < magicPlayer.Backpack.Count; i++)
            {
                AdvancedMagicItemSlot slot = new AdvancedMagicItemSlot();
                slot.index = i;
                slot.Item = magicPlayer.Backpack[i].Clone();

                enchantmentsGrid.Add(slot);
                ItemSlots.Add(slot);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var magicPlayer = Main.LocalPlayer.GetModPlayer<AdvancedMagicPlayer>();
            if (enchantmentsGrid._items.Count < magicPlayer.Backpack.Count)
            {
                AdvancedMagicItemSlot slot = new AdvancedMagicItemSlot();
                slot.index = enchantmentsGrid._items.Count;
                slot.Item = magicPlayer.Backpack[slot.index].Clone();
                enchantmentsGrid.Add(slot);
                ItemSlots.Add(slot);
            }

            //Constantly lock the UI in the position regardless of resolution changes
            Left.Pixels = RelativeLeft;
            Top.Pixels = RelativeTop;
        }
    }
}

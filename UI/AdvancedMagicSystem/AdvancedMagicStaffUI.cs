using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI.Elements;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicStaffUI : UIPanel
    {
        private UIGrid _enchantmentsGrid;

        internal const int width = 480;
        internal const int height = 155;

        internal int RelativeLeft => Main.screenWidth - 555;
        internal int RelativeTop => Main.screenHeight / 2 - 256;

        public List<AdvancedMagicStaffSlot> StaffSlots { get; private set; } = new();

        public override void OnInitialize()
        {
            base.OnInitialize();
        
            Width.Pixels = width;
            Height.Pixels = height;
            Top.Pixels = int.MaxValue / 2;
            Left.Pixels = int.MaxValue / 2;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;

            _enchantmentsGrid = new UIGrid();
            _enchantmentsGrid.Width.Set(0, 1f);
            _enchantmentsGrid.Height.Set(0, 1f);
            _enchantmentsGrid.HAlign = 0.5f;
            _enchantmentsGrid.ListPadding = 2f;
            Append(_enchantmentsGrid);
        }

        public void ReSetupSlots()
        {
            var item = AdvancedMagicUISystem.Staff;
            if (item == null)
                return;

            for (int i =0;i < StaffSlots.Count; i++)
            {
                StaffSlots[i].Item = item.EquippedEnchantments[i].Clone();
            }
        }

        public void SetupSlots()
        {
            _enchantmentsGrid.Clear();
            var item = AdvancedMagicUISystem.Staff;
            if (item == null)
                return;

            for(int i = 0; i < item.GetNormalSlotCount(); i++)
            {
                AdvancedMagicStaffSlot slot = new AdvancedMagicStaffSlot();
                slot.index = _enchantmentsGrid._items.Count;
                slot.Item = item.EquippedEnchantments[i].Clone();

                _enchantmentsGrid.Add(slot);
                StaffSlots.Add(slot);
            }

            for(int i = 0;  i < item.GetTimedSlotCount(); i++)
            {
                AdvancedMagicStaffSlot slot = new AdvancedMagicStaffSlot();
                slot.index = _enchantmentsGrid._items.Count;
                slot.Item = item.EquippedEnchantments[item.GetNormalSlotCount() + i].Clone();
                
                _enchantmentsGrid.Add(slot);
                StaffSlots.Add(slot);
            }
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

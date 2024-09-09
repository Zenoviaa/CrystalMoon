using CrystalMoon.Content.MoonlightMagic;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicStaffUI : UIPanel
    {
        private UIGrid _enchantmentsGrid;
        private BaseStaff ActiveStaff => AdvancedMagicUISystem.Staff;

        internal const int width = 480;
        internal const int height = 155;

        internal int RelativeLeft => Main.screenWidth - 555;
        internal int RelativeTop => Main.screenHeight / 2 - 256;

        public List<AdvancedMagicStaffSlot> StaffSlots { get; private set; } = new();
 
        internal AdvancedMagicStaffUI() : base()
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

            _enchantmentsGrid = new UIGrid();
            _enchantmentsGrid.Width.Set(0, 1f);
            _enchantmentsGrid.Height.Set(0, 1f);
            _enchantmentsGrid.HAlign = 0.5f;
            _enchantmentsGrid.ListPadding = 2f;
            Append(_enchantmentsGrid);
        }


        public override void OnActivate()
        {
            base.OnActivate();
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            if (!Main.gameMenu)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
        }

        public override void Recalculate()
        {
            var staff = ActiveStaff;
            Left.Pixels = RelativeLeft;
            Top.Pixels = RelativeTop;
            _enchantmentsGrid?.Clear();
            StaffSlots.Clear();
            if (staff == null)
                return;

            for (int i = 0; i < staff.GetNormalSlotCount(); i++)
            {
                AdvancedMagicStaffSlot slot = new AdvancedMagicStaffSlot(staff);
                slot.index = _enchantmentsGrid._items.Count;
                slot.Item = staff.equippedEnchantments[i].Clone();

                _enchantmentsGrid.Add(slot);
                StaffSlots.Add(slot);
            }

            for (int i = 0; i < staff.GetTimedSlotCount(); i++)
            {
                AdvancedMagicStaffSlot slot = new AdvancedMagicStaffSlot(staff);
                slot.index = _enchantmentsGrid._items.Count;
                slot.Item = staff.equippedEnchantments[staff.GetNormalSlotCount() + i].Clone();
                slot.isTimedSlot = true;
                _enchantmentsGrid.Add(slot);
                StaffSlots.Add(slot);
            }
            _enchantmentsGrid.Recalculate();
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

using CrystalMoon.Content.MoonlightMagic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CrystalMoon.UI.AdvancedMagicSystem
{
    [Autoload(Side = ModSide.Client)]
    internal class AdvancedMagicUISystem : ModSystem
    {
        private static UserInterface _backpackInterface;
        private static UserInterface _staffInterface;
        private static UserInterface _btnInterface;
        private static AdvancedMagicItemUI _itemUI;
        private static AdvancedMagicStaffUI _staffUI;
        private static AdvancedMagicElementUI _elementUI;
        private static AdvancedMagicButtonUI _buttonUI;

        public static BaseStaff Staff { get; private set; }
        private GameTime _lastUpdateUiGameTime;
        public override void Load()
        {
            base.Load();
            if (!Main.dedServ)
            {
                _backpackInterface = new UserInterface();
                _staffInterface = new UserInterface();
                _btnInterface = new UserInterface();
            }
        }

        public override void Unload()
        {
            _backpackInterface = null;
            _btnInterface = null;
            _staffInterface = null;
        }

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();
            /*if(_staffInterface.CurrentState != null)
            {
                SaveEnchantmentsToStaff();
            }
            if(_backpackInterface.CurrentState != null)
            {
                SaveBackpackToPlayer();
            }*/
        }

        internal static void OpenUI(BaseStaff baseAdvancedMagicItem)
        {
            bool resetup = false;
            if(Staff != baseAdvancedMagicItem)
            {
                Staff = baseAdvancedMagicItem;
                resetup = true;
            }
     
            if(_staffInterface.CurrentState == null)
            {
                OpenStaffUI();
                _staffUI.SetupSlots();
                _elementUI.SetupSlot();
                if (_backpackInterface.CurrentState == null)
                {
                    OpenBackpackUI();
                }
            }
            else if (resetup)
            {

                _staffUI.ReSetupSlots();
                _elementUI.SetupSlot();
                if (_backpackInterface.CurrentState == null)
                {
                    OpenBackpackUI();
                }
            } else if (!resetup)
            {
                CloseStaffUI();
                CloseBackpackUI();
            }

       
        }

        internal static void ToggleUI()
        {
            if (_backpackInterface.CurrentState != null)
            {
                CloseBackpackUI();
            }
            else
            {
                OpenBackpackUI();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if(_btnInterface?.CurrentState == null && Main.playerInventory)
            {
                OpenButtonUI();
            }
            else if(_btnInterface?.CurrentState != null && !Main.playerInventory)
            {
                CloseButtonUI();
            }

            if(!Main.playerInventory && _backpackInterface?.CurrentState != null)
            {
                CloseBackpackUI();
            }

            if (!Main.playerInventory && _staffInterface?.CurrentState != null)
            {
                CloseStaffUI();
            }

            _itemUI?.Activate();
            _lastUpdateUiGameTime = gameTime;
            if (_backpackInterface?.CurrentState != null)
            {
                _backpackInterface.Update(gameTime);
            }
            if (_staffInterface?.CurrentState != null)
            {
                _staffInterface.Update(gameTime);
            }
            if (_btnInterface?.CurrentState != null)
            {
                _btnInterface.Update(gameTime);
            }
        }

        public override void PreSaveAndQuit()
        {
            //Calls Deactivate and drops the item
            if (_backpackInterface.CurrentState != null)
            {
             //   RenamePetUI.saveItemInUI = true;
                _backpackInterface.SetState(null);
            }
        }

        internal static void OpenButtonUI()
        {
            _buttonUI = new();
            UIState state = new UIState();
            state.Append(_buttonUI);
            state.Activate();
            _buttonUI.Activate();
            _btnInterface.SetState(state);
        }

        internal static void CloseButtonUI()
        {
            _buttonUI = null;
            _btnInterface.SetState(null);
        }

        internal static void ToggleStaffUI()
        {
            if (_staffInterface.CurrentState != null)
            {
                CloseStaffUI();
            }
            else
            {
                OpenStaffUI();
            }
        }

        internal static void OpenStaffUI()
        {
            _staffUI = new();
            _elementUI = new();

            UIState state = new UIState();
            state.Append(_staffUI);
            state.Append(_elementUI);
            state.Activate();

            _staffUI.Activate();
            _elementUI.Activate();
            _staffInterface.SetState(state);
        }

        internal static void CloseStaffUI()
        {
            SaveEnchantmentsToStaff();
            _elementUI = null;
            _staffUI = null;
            _staffInterface.SetState(null);
        }

        internal static void OpenBackpackUI()
        {
            _itemUI = new();      
    
            UIState state = new UIState();
            state.Append(_itemUI); 
            state.Activate();

            _itemUI.Activate();
            _backpackInterface.SetState(state);
        }

        internal static void CloseBackpackUI()
        {
            SaveBackpackToPlayer();
            _itemUI = null;
            _backpackInterface.SetState(null);
        }


        internal static void SaveEnchantmentsToStaff()
        {
            var staff = Staff;
            if (staff == null)
                return;

 
            if(_elementUI != null)
            {
                var elementSlot = _elementUI.ElementSlot;
                staff.PrimaryElement = elementSlot.Item.Clone();
            }

            if (_staffUI != null)
            {
                var slots = _staffUI.StaffSlots;
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    staff.EquippedEnchantments[i] = slot.Item.Clone();
                }
            }
        }

        internal static void SaveBackpackToPlayer()
        {
            var player = Main.LocalPlayer.GetModPlayer<AdvancedMagicPlayer>();
            if (_itemUI == null)
                return;

            var slots = _itemUI.ItemSlots;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                player.Backpack[i] = slot.Item.Clone();
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "CrystalMoon: Advanced Magic UI",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && _backpackInterface?.CurrentState != null)
                        {
                            _backpackInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        if (_lastUpdateUiGameTime != null && _staffInterface?.CurrentState != null)
                        {
                            _staffInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        if (_lastUpdateUiGameTime != null && _btnInterface?.CurrentState != null)
                        {
                            _btnInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }
    }
}

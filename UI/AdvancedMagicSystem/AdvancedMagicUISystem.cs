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

        public static AdvancedMagicItemUI ItemUI { get; private set; }
        public static AdvancedMagicStaffUI StaffUI { get; private set; }
        public static AdvancedMagicElementUI ElementUI { get; private set; }
        public static AdvancedMagicButtonUI ButtonUI { get; private set; }
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
       
            _backpackInterface?.SetState(null);
            _btnInterface?.SetState(null);
            _staffInterface?.SetState(null);
            
            _backpackInterface = null;
            _btnInterface = null;
            _staffInterface = null;

            Staff = null;
            ItemUI = null;
            StaffUI = null;
            ElementUI = null;
            ButtonUI = null;
        }

        internal static void OpenUI(BaseStaff staff)
        {
            if(Staff != staff)
            {
                CloseStaffUI();
                Staff = staff;
                OpenStaffUI(staff);
                if (_backpackInterface.CurrentState == null)
                {
                    OpenBackpackUI();
                }
            }
            else
            {
                CloseStaffUI();
                CloseBackpackUI();
                Staff = null;
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
                _staffInterface.SetState(null);
                _btnInterface.SetState(null);
            }
        }

        internal static void OpenButtonUI()
        {
            //New Button
            ButtonUI = new();      

            //Setup UI State
            UIState state = new UIState();
            state.Append(ButtonUI);
            state.Activate();

            //Set State
            _btnInterface.SetState(state);
        }

        internal static void CloseButtonUI()
        {
            //Kill
            ButtonUI = null;
            _btnInterface.SetState(null);
        }

        internal static void OpenStaffUI(BaseStaff staff)
        {
            //Initialize UI
            ElementUI = new(staff);
            StaffUI = new(staff);
      
            //Setup UI State
            UIState state = new UIState();
            state.Append(ElementUI);
            state.Append(StaffUI);
            state.Activate();

            //Set State
            _staffInterface.SetState(state);
        }

        internal static void CloseStaffUI()
        {
            ElementUI?.Deactivate();
            StaffUI?.Deactivate();
            ElementUI = null;
            StaffUI = null;
            _staffInterface.SetState(null);
        }

        internal static void OpenBackpackUI()
        {
            //Initialize UI
            ItemUI = new();
            
            //Setup UI State
            UIState state = new UIState();
            state.Append(ItemUI); 
            state.Activate();

            //Set State
            _backpackInterface.SetState(state);
        }

        internal static void CloseBackpackUI()
        {
            ItemUI?.Deactivate();
            ItemUI = null;
            _backpackInterface.SetState(null);
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

﻿using CrystalMoon.Content.MoonlightMagic;
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
        private UserInterface _backpackInterface;
        private UserInterface _staffInterface;
        private UserInterface _btnInterface;

        public StaffUIState staffUIState;
        public ItemUIState itemUIState;
        public ButtonUIState buttonUIState;

        public static BaseStaff Staff { get; private set; }
        private GameTime _lastUpdateUiGameTime;

        
        public override void OnModLoad()
        {
            base.OnModLoad();
            _backpackInterface = new UserInterface();
            _staffInterface = new UserInterface();
            _btnInterface = new UserInterface();

            staffUIState = new StaffUIState();
            itemUIState = new ItemUIState();
            buttonUIState = new ButtonUIState();

          
            staffUIState.Activate();
            itemUIState.Activate();
            buttonUIState.Activate();
        }

        internal void Recalculate()
        {
            staffUIState?.staffUI?.Recalculate();
            staffUIState?.elementUI?.Recalculate();
            staffUIState?.elementUI?.ElementSlot?.Refresh();
            itemUIState?.itemUI?.Recalculate();
        }

        internal void OpenUI(BaseStaff staff)
        {
            if(Staff != staff)
            {
                CloseStaffUI();
                Staff = staff;
                Recalculate();
                OpenStaffUI();
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

        internal void ToggleUI()
        {
            Console.WriteLine("Toggle UI");
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

        internal void OpenButtonUI()
        {
            //Set State
            _btnInterface.SetState(buttonUIState);
        }

        internal void CloseButtonUI()
        {
            //Kill
            _btnInterface.SetState(null);
        }

        internal void OpenStaffUI()
        {
            //Set State
            _staffInterface.SetState(staffUIState);
        }

        internal void CloseStaffUI()
        {
            _staffInterface.SetState(null);
        }

        internal void OpenBackpackUI()
        {
            //Set State
            _backpackInterface.SetState(itemUIState);
        }

        internal void CloseBackpackUI()
        {
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

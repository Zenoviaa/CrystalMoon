﻿using CrystalMoon.Content.MoonlightMagic;
using CrystalMoon.Systems.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;
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

        public List<AdvancedMagicStaffSlot> StaffSlots { get; private set; } = new();
 
        internal AdvancedMagicStaffUI() : base()
        {
  
        }
        private string AssetDirectory = $"CrystalMoon/UI/AdvancedMagicSystem/";
        private Texture2D
            _enchantmentPanel;

        public override void OnActivate()
        {
            base.OnActivate();
            _enchantmentPanel = ModContent.Request<Texture2D>($"{AssetDirectory}EnchantmentPanel").Value;
        
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            Width.Pixels = width;
            Height.Pixels = height;
            SetPos();
        
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;

            _enchantmentsGrid = new UIGrid();
            _enchantmentsGrid.Width.Set(0, 1f);
            _enchantmentsGrid.Height.Set(0, 1f);
            _enchantmentsGrid.HAlign = 0.5f;
            _enchantmentsGrid.ListPadding = 2f;
            Append(_enchantmentsGrid);
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
            SetPos();
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

        private void SetPos()
        {
            var config = ModContent.GetInstance<CrystalMoonClientConfig>();
            Vector2 ratioPos = new Vector2(config.EnchantMenuX, config.EnchantMenuY);
            if (ratioPos.X < 0f || ratioPos.X > 100f)
            {
                ratioPos.X = 50;
            }

            if (ratioPos.Y < 0f || ratioPos.Y > 100f)
            {
                ratioPos.Y = 3;
            }

            Vector2 drawPos = ratioPos;
            drawPos.X = (int)(drawPos.X * 0.01f * Main.screenWidth);
            drawPos.Y = (int)(drawPos.Y * 0.01f * Main.screenHeight);

            Left.Pixels = drawPos.X;
            Top.Pixels = drawPos.Y;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Constantly lock the UI in the position regardless of resolution changes
            SetPos();
        }


        public Color Color = Color.White;
        public bool ScaleToFit = false;
        public float ImageScale = 1f;
        public float Rotation;
        public bool AllowResizingDimensions = true;
        public Vector2 NormalizedOrigin = Vector2.Zero;
        private static Vector2? _drag = null;
        private static bool _isDragging;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            var config = ModContent.GetInstance<CrystalMoonClientConfig>();
            Vector2 vector = _enchantmentPanel.Size();
            Vector2 ratioPos = new Vector2(config.EnchantMenuX, config.EnchantMenuY);
            if (ratioPos.X < 0f || ratioPos.X > 100f)
            {
                ratioPos.X = 50;
            }

            if (ratioPos.Y < 0f || ratioPos.Y > 100f)
            {
                ratioPos.Y = 3;
            }

            Vector2 drawPos = ratioPos;
            drawPos.X = (int)(drawPos.X * 0.01f * Main.screenWidth);
            drawPos.Y = (int)(drawPos.Y * 0.01f * Main.screenHeight);

            Rectangle mouseRect = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 8, 8);
            Vector2 size = new Vector2(480, 155);
            Rectangle barRect = Utils.CenteredRectangle(drawPos + size / 2, size * Main.UIScale);

            MouseState ms = Mouse.GetState();
            Vector2 mousePos = Main.MouseScreen;
            Vector2 newScreenRatioPosition = ratioPos;
            if (ms.LeftButton == ButtonState.Pressed && !_isDragging && barRect.Intersects(mouseRect))
            {
                _isDragging = true;
            }
            spriteBatch.Draw(_enchantmentPanel, drawPos, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, SpriteEffects.None, 0f);
            //Handle dragging
            if (_isDragging)
            {
                if (!_drag.HasValue)
                    _drag = mousePos - drawPos;

                Vector2 newCorner = mousePos - _drag.GetValueOrDefault(Vector2.Zero);

                // Convert the new corner position into a screen ratio position.
                newScreenRatioPosition.X = (100f * newCorner.X) / Main.screenWidth;
                newScreenRatioPosition.Y = (100f * newCorner.Y) / Main.screenHeight;

                // Compute the change in position. If it is large enough, actually move the meter
                Vector2 delta = newScreenRatioPosition - ratioPos;
                if (Math.Abs(delta.X) >= 0.05f || Math.Abs(delta.Y) >= 0.05f)
                {
                    config.EnchantMenuX = newScreenRatioPosition.X;
                    config.EnchantMenuY = newScreenRatioPosition.Y;
                }

                if (ms.LeftButton == ButtonState.Released)
                {
                    _isDragging = false;
                    _drag = null;
                    MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
                    if (saveMethodInfo is not null)
                        saveMethodInfo.Invoke(null, new object[] { config });
                }
            }
        }
    }
}

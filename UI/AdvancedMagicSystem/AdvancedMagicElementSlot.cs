using CrystalMoon.Content.MoonlightMagic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;


namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicElementSlot : UIElement
    {

        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        internal event Action<int> OnEmptyMouseover;

        private int timer = 0;

        internal AdvancedMagicElementSlot(int context = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);

            var inventoryBack9 = TextureAssets.InventoryBack9;
            Width.Set(inventoryBack9.Width() * scale, 0f);
            Height.Set(inventoryBack9.Height() * scale, 0f);
        }

        /// <summary>
        /// Returns true if this item can be placed into the slot (either empty or a pet item)
        /// </summary>
        internal bool Valid(Item item)
        {
            return item.ModItem is BaseElement || item.IsAir;
        }

        internal void HandleMouseItem()
        {
            if (Valid(Main.mouseItem))
            {
                //Handles all the click and hover actions based on the context
     
                ItemSlot.Handle(ref Item, _context);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();
       
            bool contains = ContainsPoint(Main.MouseScreen);

            if (contains && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                HandleMouseItem();
            }

            //Draw Backing
            Color color2 = Main.inventoryBack;
            Vector2 pos = rectangle.TopLeft();
     
            Texture2D value = ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/ElementSlot").Value;

            Vector2 centerPos = pos + rectangle.Size() / 2f;
            spriteBatch.Draw(value, rectangle.TopLeft(), null, color2, 0f, default(Vector2), _scale, SpriteEffects.None, 0f);
            ItemSlot.DrawItemIcon(Item, _context, spriteBatch, centerPos + new Vector2(12, 12), _scale, 64, Color.White);

            if (contains && Item.IsAir)
            {
                timer++;
                OnEmptyMouseover?.Invoke(timer);
            }
            else if (!contains)
            {
                timer = 0;
            }

            Main.inventoryScale = oldScale;
        }
    }
}

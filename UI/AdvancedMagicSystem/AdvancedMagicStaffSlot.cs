using CrystalMoon.Content.MoonlightMagic;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;


namespace CrystalMoon.UI.AdvancedMagicSystem
{
    internal class AdvancedMagicStaffSlot : UIElement
    {
        private Item _prevItem;
        private readonly BaseStaff _staff;
        private readonly int _context;
        private readonly float _scale;

        internal Item Item;
        internal Func<Item, bool> ValidItemFunc;

        internal event Action<int> OnEmptyMouseover;

        private int timer = 0;

        internal AdvancedMagicStaffSlot(BaseStaff staff, int context = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            _staff = staff;
            Item = new Item();
            Item.SetDefaults(0);

            var asset = ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/EnchantmentCard");
            Width.Set(asset.Width() * scale, 0f);
            Height.Set(asset.Height() * scale, 0f);
    
        }

        public int index;
        public bool isTimedSlot;
        /// <summary>
        /// Returns true if this item can be placed into the slot (either empty or a pet item)
        /// </summary>
        internal bool Valid(Item item)
        {
            if (isTimedSlot)
            {
                if(item.ModItem is BaseEnchantment enchantment && enchantment.isTimedEnchantment)
                {
                    return true;
                }
                if (item.IsAir)
                {
                    return true;
                }
            }
            else
            {
                if(item.ModItem is BaseEnchantment enchantment && !enchantment.isTimedEnchantment)
                {
                    return true;
                }

                if (item.IsAir)
                {
                    return true;
                }
            }

            return false;
        }

        internal void HandleMouseItem()
        {
            if (Valid(Main.mouseItem))
            {
                _prevItem = Item;
                //Handles all the click and hover actions based on the context
                ItemSlot.Handle(ref Item, _context);
                if(Item != _prevItem)
                {
                    SaveToStaff();
                }
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
            bool isMatch = false;
            Color color2 = Main.inventoryBack;
            Vector2 pos = rectangle.TopLeft();

            //Enchantment Card
            var uiSystem = ModContent.GetInstance<AdvancedMagicUISystem>();
            var elementItem = uiSystem.staffUIState.elementUI.ElementSlot.Item;
            if (Item.ModItem is BaseEnchantment enchantment &&
               elementItem.type == enchantment.GetElementType())
            {
                if (elementItem.ModItem is BaseElement element)
                {
                    isMatch = true;
                    color2 = element.GetElementColor();
                }
            }
            else
            {
                color2 = Color.LightGoldenrodYellow;
            }

            Texture2D cardTexture = isTimedSlot 
                ? ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/TimedEnchantmentCard").Value
                : ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/EnchantmentCard").Value;

            int offset = (int)(cardTexture.Size().Y / 2);
            Vector2 centerPos = pos + rectangle.Size() / 2f;
            spriteBatch.Draw(cardTexture, rectangle.TopLeft(), null, color2, 0f, default(Vector2), _scale, SpriteEffects.None, 0f);

            if(Item.ModItem is BaseEnchantment myEnchantment)
            {
                var myElement = ModContent.GetModItem(myEnchantment.GetElementType()) as BaseElement;
                var shader = FirePixelShader.Instance;
                shader.PrimaryColor = Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f);
                shader.NoiseColor = myElement.GetElementColor();
                shader.Distortion = 0.0075f;
                shader.Speed = 10;
                shader.Power = 0.01f;
                shader.Apply();

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, default, Main.UIScaleMatrix);

                shader.Data.Apply(null);

                spriteBatch.Draw(cardTexture, rectangle.TopLeft(), null, color2, 0f, default(Vector2), _scale, SpriteEffects.None, 0f);

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);

            }


            //Enchantment Slot
            cardTexture = isTimedSlot 
                ? ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/TimedEnchantmentSlot").Value
                : ModContent.Request<Texture2D>("CrystalMoon/UI/AdvancedMagicSystem/EnchantmentSlot").Value;
            spriteBatch.Draw(cardTexture, rectangle.TopLeft() + new Vector2(0, 38), null, color2, 0f, default(Vector2), _scale, SpriteEffects.None, 0f);
            if (isMatch)
            {
                float sizeLimit = 34;
                int numberOfCloneImages = 6;
                Color glowColor = color2;
                Main.DrawItemIcon(spriteBatch, Item, centerPos, glowColor * 0.7f, sizeLimit);
                for (float i = 0; i < 1; i += 1f / numberOfCloneImages)
                {
                    float cloneImageDistance = MathF.Cos(Main.GlobalTimeWrappedHourly / 2.4f * MathF.Tau / 2f) + 0.5f;
                    cloneImageDistance = MathHelper.Max(cloneImageDistance, 0.1f);
                    Color color = glowColor * 0.4f;
                    color *= 1f - cloneImageDistance * 0.2f;
                    color.A = 0;
                    cloneImageDistance *= 3;
                    Vector2 drawPos = centerPos + (i * MathF.Tau).ToRotationVector2() * (cloneImageDistance + 2f);
                    Main.DrawItemIcon(spriteBatch, Item, drawPos, color, sizeLimit);
                }
            }

            ItemSlot.DrawItemIcon(Item, _context, spriteBatch, centerPos, _scale, 32, Color.White);

     
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

        public void SaveToStaff()
        {
            //Save Item 
            _staff.equippedEnchantments[index] = Item.Clone();
        }
    }
}

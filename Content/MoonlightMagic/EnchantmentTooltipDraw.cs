using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class EnchantmentTooltipDraw : GlobalItem
    {


        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {

            if (line.Mod == "CrystalMoon" && line.Name.Contains("MoonMagicEnchant_"))
            {
                line.BaseScale *= 0.75f;
                line.X += 15;
                line.Y += 6;
            }

            return base.PreDrawTooltipLine(item, line, ref yOffset);
        }

        public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
        {
            base.PostDrawTooltipLine(item, line);

            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                if(item.ModItem is BaseStaff baseStaff && baseStaff.PrimaryElement.ModItem is BaseElement)
                {
                    string elementName = baseStaff.PrimaryElement.Name;
                    elementName = elementName.Replace(" ", "");
                    Texture2D texture = ModContent.Request<Texture2D>(
                        $"CrystalMoon/Content/MoonlightMagic/Elements/{elementName}").Value;

                    SpriteBatch spriteBatch = Main.spriteBatch;
                    Vector2 textPosition = new(line.X, line.Y);
                    Vector2 drawPos = textPosition + new Vector2(0, texture.Size().Y / 3.5f) - new Vector2(25, 6);
                    spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() * 0.5f, 0.75f, SpriteEffects.None, 0f);
                }
            }

            if (line.Mod == "CrystalMoon" && line.Name.Contains("MoonMagicEnchant_"))
            {
                int startIndex = line.Name.IndexOf("_")+1;
                int endIndex = line.Name.LastIndexOf("_");
                string textureName = line.Name.Substring(startIndex, endIndex - startIndex);
                Texture2D texture = ModContent.Request<Texture2D>(
                    $"CrystalMoon/Content/MoonlightMagic/Enchantments/{textureName}").Value;

                SpriteBatch spriteBatch = Main.spriteBatch;
                Vector2 textPosition = new(line.X, line.Y);
                Vector2 drawPos = textPosition + new Vector2(0, texture.Size().Y / 3.5f) - new Vector2(15, 6);     
                spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() * 0.5f, 0.75f, SpriteEffects.None, 0f);
      
            }
        }
    }
}

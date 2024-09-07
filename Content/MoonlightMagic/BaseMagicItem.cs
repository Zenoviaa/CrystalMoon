using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseMagicItem : ModItem
    {
        public virtual void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

        }

        public override bool OnPickup(Player player)
        {
            player.GetModPlayer<AdvancedMagicPlayer>().Pickup(Item);
            return false;
        }
    }
}

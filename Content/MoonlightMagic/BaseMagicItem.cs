using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseMagicItem : ModItem
    {
        public override bool OnPickup(Player player)
        {
            player.GetModPlayer<AdvancedMagicPlayer>().Pickup(Item);
            return false;
        }
    }
}

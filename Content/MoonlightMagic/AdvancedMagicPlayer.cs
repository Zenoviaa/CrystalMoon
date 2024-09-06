using System.Collections.Generic;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class AdvancedMagicPlayer : ModPlayer
    {
        public BaseElement PrimaryElement { get; set; }
        public List<BaseEnchantment> Enchantments { get; set; } = new List<BaseEnchantment>();
    }
}

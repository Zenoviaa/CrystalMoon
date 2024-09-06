using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Content.MoonlightMagic.Enchantments;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class AdvancedMagicPlayer : ModPlayer
    {
        public BaseElement PrimaryElement { get; set; } 
            = new RadianceElement();
        public List<BaseEnchantment> Enchantments { get; set; }
            = new List<BaseEnchantment>() { new FlameLashEnchantment() };
    }
}

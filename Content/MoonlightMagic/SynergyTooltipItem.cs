using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class SynergyTooltipItem : ModItem
    {
        public override string Texture => TextureRegistry.EmptyTexturePath;
        public BaseElement PrimaryElement;
        public BaseEnchantment Enchantment;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            tooltips.Clear();
            TooltipLine tooltipLine;
            if (Enchantment != null && PrimaryElement != null)
            {
                if(Enchantment.GetElementType() == PrimaryElement.Type)
                {
                    tooltipLine = new TooltipLine(Mod, "SynergyHelp",
                                   Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonSynergy", 4));
                    tooltips.Add(tooltipLine);
                }
                else
                {
                    AddNoSynergyText(tooltips);
                }
            }
            else
            {
                AddNoSynergyText(tooltips);
            }
        }

        private void AddNoSynergyText(List<TooltipLine> tooltips)
        {
            var tooltipLine = new TooltipLine(Mod, "NoSynergyHelp",
             Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonNoSynergy"));
            tooltipLine.OverrideColor = Color.Gray;
            tooltips.Add(tooltipLine);
        }
    }
}

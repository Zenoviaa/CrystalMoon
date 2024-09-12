using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Systems.LoadingSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Deeya
{
    internal class MimicryEnchantment : BaseEnchantment
    {
        bool HasSwapped;
        public override float GetStaffManaModifier()
        {
            return 0.5f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<DeeyaElement>();
        }

        public override void AI()
        {
            base.AI();
            if (!HasSwapped)
            {
                int indexOfThisEnchantment = MagicProj.IndexOfEnchantment(this);
                if(indexOfThisEnchantment > 0)
                {
                    MagicProj.ReplaceEnchantment(MagicProj.Enchantments[indexOfThisEnchantment-1], indexOfThisEnchantment);
                }
           
                HasSwapped = true;
            }
        }
    }
}

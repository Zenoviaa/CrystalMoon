﻿using CrystalMoon.Content.MoonlightMagic.Elements;
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
    internal class GamblerEnchantment : BaseEnchantment
    {
        bool HasSwapped;
        public override float GetStaffManaModifier()
        {
            return 0.2f;
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
                var enchantmentsToSpawn = AllEnchantments;
                BaseEnchantment enchantmentToSwapTo = enchantmentsToSpawn[Main.rand.Next(0, enchantmentsToSpawn.Length)];
                int indexOfThisEnchantment = MagicProj.IndexOfEnchantment(this);
        
                MagicProj.ReplaceEnchantment(enchantmentToSwapTo, indexOfThisEnchantment);
                HasSwapped = true;
            }
        }
    }
}

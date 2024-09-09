using CrystalMoon.UI.AdvancedMagicSystem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class AdvancedMagicPlayer : ModPlayer
    {
        public List<Item> Backpack { get; set; } = new List<Item>();
        public static event Action<Item> MagicPickupEvent;

        public void Pickup(Item item)
        {
            var uiSystem = ModContent.GetInstance<AdvancedMagicUISystem>();
            bool success = false;
            int updateIndex = 0;
            for(int i = 0; i < Backpack.Count; i++)
            {
                if (Backpack[i].IsAir)
                {
                    Backpack[i] = item;
                    updateIndex = i;
                    success = true;
                    break;
                }
            }

            if (!success)
            {
                updateIndex = Backpack.Count;
                Backpack.Add(item);
            }    

            int airCount = 0;
            for (int i = 0; i < Backpack.Count; i++)
            {
                if (Backpack[i].IsAir)
                {
                    airCount++;
                }
            }

            Backpack.RemoveAll(x => x.IsAir);
            if (airCount == 0)
            {
                Item emptyItem = new Item();
                emptyItem.SetDefaults(0);
                Backpack.Add(emptyItem);
            }

            uiSystem.Recalculate();
            MagicPickupEvent?.Invoke(item);
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["magicbackpack_itemCount"] = Backpack.Count;
            for (int i = 0; i < Backpack.Count; i++)
            {
                var enchantment = Backpack[i];
                if (enchantment == null)
                    continue;
                tag[$"magicbackpack_enchantment_{i}"] = enchantment;
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.ContainsKey("magicbackpack_itemCount"))
            {
                int itemCount = tag.GetInt("magicbackpack_itemCount");
                Backpack.Clear();
                for (int i = 0; i < itemCount; i++)
                {
                    if (tag.ContainsKey($"magicbackpack_enchantment_{i}"))
                    {
                        var enchantment = tag.Get<Item>($"magicbackpack_enchantment_{i}");
                        Backpack.Add(enchantment);
                    }
                }
            }
        }
    }
}

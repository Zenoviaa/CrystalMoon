﻿using CrystalMoon.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseMagicItem : ModItem
    {
        public virtual void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

        }

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);

            Console.WriteLine($"Pickup {Item.Name}");
            player.GetModPlayer<AdvancedMagicPlayer>().Pickup(Item);
            for(int i = 0; i < player.inventory.Length; i++)
            {
                Item item = player.inventory[i];
                if(item == Item)
                {
                    player.inventory[i] = new Item();
                    player.inventory[i].SetDefaults(0);
                }
            }
        }

        public override bool OnPickup(Player player)
        {
            player.GetModPlayer<AdvancedMagicPlayer>().Pickup(Item);
            PopupText.NewText(PopupTextContext.SonarAlert, Item, 1, longText: true);
            return false;
        }
    }
}

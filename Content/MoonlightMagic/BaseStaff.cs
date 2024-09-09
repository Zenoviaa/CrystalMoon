﻿using CrystalMoon.UI.AdvancedMagicSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseStaff : ModItem
    {
        private static Item _preReforgeElement;
        private static Item[] _preReforgeEnchants;
        public Texture2D Form { get; set;}
        public BaseMovement Movement { get; set; }

        public int Size { get; set; }
        public int TrailLength { get; set; }
        public UnifiedRandom Random { get; private set; }

        //Enchantment Slots
        public Item primaryElement;
        public Item[] equippedEnchantments;

        public override void SetDefaults()
        {
            base.SetDefaults();
            equippedEnchantments = new Item[GetNormalSlotCount() + GetTimedSlotCount()];
            primaryElement = new Item();
            primaryElement.SetDefaults(0);
            for (int i = 0; i < equippedEnchantments.Length; i++)
            {
                equippedEnchantments[i] = new Item();
                equippedEnchantments[i].SetDefaults(0);
            }

            Item.damage = 18;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.mana = 10;

            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<AdvancedMagicProjectile>();
            Item.autoReuse = true;
            TrailLength = 32;
            Size = 8;

            //Randomize trail values
            int seed = WorldGen._genRandSeed;
            Random = new UnifiedRandom(seed);
            TrailLength = Random.Next(24, 32);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(player, ref damage);
            for(int i = 0; i < equippedEnchantments.Length; i++)
            {
                Item item = equippedEnchantments[i];
                if(item.ModItem is BaseEnchantment enchantment)
                {
                    if(enchantment.GetElementType() == primaryElement.type)
                    {
            
                        damage *= 1.1f;
                        //damage *= 1.14f;
                    }
                }
            }
        }


        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            base.ModifyManaCost(player, ref reduce, ref mult);
            for(int i = 0; i < equippedEnchantments.Length; i++)
            {
                Item item = equippedEnchantments[i];
                if(item.ModItem is BaseEnchantment enchantment)
                {
                    mult += enchantment.GetStaffManaModifier();
                }
            }
        }

        public override void PreReforge()
        {
            base.PreReforge();
            
            _preReforgeElement = primaryElement.Clone();
            _preReforgeEnchants = new Item[equippedEnchantments.Length];
            for(int i = 0; i < _preReforgeEnchants.Length; i++)
            {
                _preReforgeEnchants[i] = equippedEnchantments[i].Clone();
            }
        }

        public override void PostReforge()
        {
            base.PostReforge();
            
            primaryElement = _preReforgeElement;
            equippedEnchantments = _preReforgeEnchants;
            
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            TooltipLine tooltipLine;

            tooltipLine = new TooltipLine(Mod, "WeaponType",
                Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonStaff"));
            tooltipLine.OverrideColor = Color.White;
            tooltips.Add(tooltipLine);


            for (int i = 0; i < equippedEnchantments.Length; i++)
            {
                var item = equippedEnchantments[i];
                if(item.ModItem is BaseEnchantment enchantment)
                {
                    tooltipLine = new TooltipLine(Mod, $"MoonMagicEnchant_{enchantment.Texture}_{i}", enchantment.DisplayName.Value);
                    tooltips.Add(tooltipLine);
                }
            }
        }

        public virtual int GetNormalSlotCount()
        {
            return 5;
        }

        public virtual int GetTimedSlotCount()
        {
            return 2;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override bool ConsumeItem(Player player) => false;

        public override void RightClick(Player player)
        {
            base.RightClick(player);
            ModContent.GetInstance<AdvancedMagicUISystem>().OpenUI(this);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if(primaryElement.ModItem is BaseElement element)
            {
                element.SpecialInventoryDraw(Item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            }

            for(int i = 0; i < equippedEnchantments.Length; i++)
            {
                var enchant = equippedEnchantments[i];
                if(enchant.ModItem is BaseEnchantment enchantment)
                {
                    enchantment.SpecialInventoryDraw(Item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
                }
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            AdvancedMagicUtil.NewMagicProjectile(this, player, source, position, velocity, type, damage, knockback);
            return false;
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["itemCount"] = equippedEnchantments.Length;
            if (primaryElement != null)
                tag["element"] = primaryElement;
            for(int i = 0; i < equippedEnchantments.Length; i++)
            {
                var enchantment = equippedEnchantments[i];
                if (enchantment == null)
                    continue;
                tag[$"enchantment_{i}"] = enchantment;
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.ContainsKey("element"))
            {
                var element = tag.Get<Item>("element");
                primaryElement = element;
            }

            if (tag.ContainsKey("itemCount"))
            {
                int itemCount = tag.GetInt("itemCount");
                equippedEnchantments = new Item[itemCount];
                for (int i = 0; i < itemCount; i++)
                {
                    if (tag.ContainsKey($"enchantment_{i}"))
                    {
                        var enchantment = tag.Get<Item>($"enchantment_{i}");
                        equippedEnchantments[i] = enchantment;
                    }       
                }
            }
        }
    }
}

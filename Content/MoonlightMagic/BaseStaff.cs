using CrystalMoon.UI.AdvancedMagicSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseStaff : ModItem
    {
        public Texture2D Form { get; set;}
        public BaseMovement Movement { get; set; }
        public int TrailLength { get; set; }
        public UnifiedRandom Random { get; private set; }

        //Enchantment Slots
        public Item PrimaryElement { get; set; }
        public Item[] EquippedEnchantments { get; set; }
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            EquippedEnchantments = new Item[GetNormalSlotCount() + GetTimedSlotCount()];
            PrimaryElement = new Item();
            PrimaryElement.SetDefaults(0);
            for (int i = 0; i < EquippedEnchantments.Length; i++)
            {
                EquippedEnchantments[i] = new Item();
                EquippedEnchantments[i].SetDefaults(0);
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

            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<AdvancedMagicProjectile>();
            Item.autoReuse = true;
            TrailLength = 32;

            //Randomize trail values
            int seed = WorldGen._genRandSeed;
            Random = new UnifiedRandom(seed);
            TrailLength = Random.Next(24, 32);
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
            AdvancedMagicUISystem.OpenUI(this);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            AdvancedMagicUtil.NewMagicProjectile(this, player, source, position, velocity, type, damage, knockback);
            return false;
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["itemCount"] = EquippedEnchantments.Length;
            if (PrimaryElement != null)
                tag["element"] = PrimaryElement;
            for(int i = 0; i < EquippedEnchantments.Length; i++)
            {
                var enchantment = EquippedEnchantments[i];
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
                PrimaryElement = element;
            }

            if (tag.ContainsKey("itemCount"))
            {
                int itemCount = tag.GetInt("itemCount");
                EquippedEnchantments = new Item[itemCount];
                for (int i = 0; i < itemCount; i++)
                {
                    if (tag.ContainsKey($"enchantment_{i}"))
                    {
                        var enchantment = tag.Get<Item>($"enchantment_{i}");
                        EquippedEnchantments[i] = enchantment;
                    }       
                }
            }
        }
    }
}

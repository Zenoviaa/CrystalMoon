using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseEnchantment : BaseMagicItem, 
        IAdvancedMagicAddon,
        ICloneable
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        public override string LocalizationCategory => "Enchantments";

        public int time;
        public bool isTimedEnchantment => time > 0;
        public virtual float GetStaffManaModifier() { return 0.2f; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public BaseEnchantment Instantiate()
        {
            return (BaseEnchantment)Clone(); 
        }

        public virtual int GetElementType()
        {
            return ModContent.ItemType<BasicElement>();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            TooltipLine tooltipLine = new TooltipLine(Mod, "EnchantmentHelp",
                Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonHelp"));
            tooltipLine.OverrideColor = Color.Gray;
            tooltips.Add(tooltipLine);

            if (isTimedEnchantment)
            {
                tooltipLine = new TooltipLine(Mod, "EnchantmentTimedHelp",
                    Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonTimed", time));
                tooltips.Add(tooltipLine);
            }

            tooltipLine = new TooltipLine(Mod, "EnchantmentManaHelp",
                Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonMana", 
                GetStaffManaModifier() * 100));
            tooltipLine.OverrideColor = Color.IndianRed;
            tooltips.Add(tooltipLine);


            /*
            tooltipLine = new TooltipLine(Mod, "EnchantmentTooltip",
                Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonTooltip"));
            tooltips.Add(tooltipLine);*/
        }

        public virtual void DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset) { }   public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            DrawHelper.DrawGlow2InWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        //Enchantment stuff
        public virtual void AI() { }
        public virtual void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
        public virtual void OnKill(int timeLeft) { }
        public virtual void OnTileCollide(Vector2 oldVelocity) { }
    }
    
}

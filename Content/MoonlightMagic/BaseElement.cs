using CrystalMoon.Systems.ScreenSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseElement : BaseMagicItem,
        ICloneable,
        IAdvancedMagicAddon
    {
        public AdvancedMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override string LocalizationCategory => "Elements";

        public virtual void AI() { }
        public virtual void DrawTrail() { }
        public virtual void ApplyFormShader() { }

        public virtual Color GetElementColor() { return Color.White; }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            TooltipLine tooltipLine = new TooltipLine(Mod, "EnchantmentHelp",
                Language.GetTextValue("Mods.CrystalMoon.Enchantments.EnchantmentCommonHelp"));
            tooltipLine.OverrideColor = Color.Gray;
            tooltips.Add(tooltipLine);
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            DrawHelper.DrawGlow2InWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public virtual bool DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset)
        {
            return false;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public BaseElement Instantiate()
        {
            return (BaseElement)Clone();
        }
    }
}

﻿using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Systems.ScreenSystems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CrystalMoon.Registries;
using CrystalMoon.Content.MoonlightMagic.Movements;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Visual.Particles;
using CrystalMoon.Systems.MiscellaneousMath;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Uvilis
{
    internal class TyphoonWaverEnchantment : BaseEnchantment
    {
        private float _timer;

        public override void AI()
        {
            base.AI();

            //Count up
            _timer++ ;
            float oscSpeed = 0.05f;
            float xAmp = 0.5f;
            float yAmp = 0.5f;
            Projectile.velocity += new Vector2(
                MathF.Sin(_timer * oscSpeed) * xAmp,
                MathF.Cos(_timer * oscSpeed) * yAmp);
            //If greater than time then start homing, we'll just swap the movement type of the projectile

        }

        public override float GetStaffManaModifier()
        {
            return 0.3f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<UvilisElement>();
        }


        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.PhantasmalGreen);
        }
    }
}

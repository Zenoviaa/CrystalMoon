using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
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

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Phantasmal
{
    internal class MoonlightSwirlEnchantment : BaseEnchantment
    {
        private float _timer;

        public override void AI()
        {
            base.AI();

            //Count up
            _timer++;
            float oscSpeed = 0.01f;
            float xAmp = 15f;
            float yAmp = 15f;

            Vector2 circleVelocity = new Vector2(
                MathF.Sin(_timer * oscSpeed) * xAmp,
                MathF.Cos(_timer * oscSpeed) * yAmp);
            circleVelocity = circleVelocity.RotatedBy(Projectile.velocity.ToRotation());
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, circleVelocity, 0.5f);
            //If greater than time then start homing, we'll just swap the movement type of the projectile

        }

        public override float GetStaffManaModifier()
        {
            return 0.3f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<PhantasmalElement>();
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

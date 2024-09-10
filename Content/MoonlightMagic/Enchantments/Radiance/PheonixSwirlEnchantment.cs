using CrystalMoon.Content.MoonlightMagic.Elements;
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

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance
{
    internal class PheonixSwirlEnchantment : BaseEnchantment
    {
        private float _timer;
        private Vector2 _velocity;
        public override void AI()
        {
            base.AI();

            //Count up
            _timer++ ;
            if(_timer == 1)
            {
                _velocity = Projectile.velocity;
            }

            Vector2 newVelocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(10));
            Projectile.velocity = newVelocity;
            Projectile.Center += _velocity * 0.3f;

            // Projectile.a greater than time then start homing, we'll just swap the movement type of the projectile

        }


        public override float GetStaffManaModifier()
        {
            return 0.3f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<RadianceElement>();
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

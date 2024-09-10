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
            Projectile.velocity += (Projectile.velocity.SafeNormalize(Vector2.Zero) * 4).RotatedBy(MathHelper.ToRadians(15 * MathF.Sin(_timer *= 0.05f)));
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

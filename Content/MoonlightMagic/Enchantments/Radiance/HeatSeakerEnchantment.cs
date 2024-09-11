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

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance
{
    internal class HeatSeakerEnchantment : BaseEnchantment
    {
        private int _timer;
        public override void SetDefaults()
        {
            base.SetDefaults();
            
        }

        public override void AI()
        {
            base.AI();

            //Count up
            _timer++;

            //If greater than time then start homing, we'll just swap the movement type of the projectile
            if (_timer == 1)
            {

                MagicProj.Movement = new HomingMovement();
            }
            MagicProj.Size *= 1.005f;
            Projectile.velocity *= 1.005f;
        }

        public override float GetStaffManaModifier()
        {
            return 0.4f;
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
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.RadianceYellow);
        }
    }
}

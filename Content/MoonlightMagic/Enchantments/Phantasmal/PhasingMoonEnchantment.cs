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
    internal class PhasingMoonEnchantment : BaseEnchantment
    {
        private float _timer;



        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override float GetStaffManaModifier()
        {
            return 0.1f;
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

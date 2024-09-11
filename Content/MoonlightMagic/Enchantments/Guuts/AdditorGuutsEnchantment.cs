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
using System.Threading;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Guuts
{
    internal class AdditorGuutsEnchantment : BaseEnchantment
    {
        
        public override void SetDefaults()
        {
            base.SetDefaults();
          
        }

        private bool Decresed = false;
        
        public override void AI()
        {
            base.AI();

            //Count up
    

            //If greater than time then start homing, we'll just swap the movement type of the projectile
            if (!Decresed)
            {
                foreach (var enchantment in MagicProj.Enchantments)
                {
                    //do a thing here
                   
                      enchantment.time += 15;
                    

                }
                Decresed = true;
            }
               
            

        }

        public override float GetStaffManaModifier()
        {
            return 1f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType <GuutElement>();
        }


        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.GuutGray);
        }
    }
}

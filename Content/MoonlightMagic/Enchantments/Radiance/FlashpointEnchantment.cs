using CrystalMoon.Content.Bases;
using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance
{
    internal class FlashpointEnchantment : BaseEnchantment
    {
        public override float GetStaffManaModifier()
        {
            return 0.2f;
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
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, Color.Red);
        }

        public override void AI()
        {
            Projectile.velocity *= 1.02f;

        }
    }



    }

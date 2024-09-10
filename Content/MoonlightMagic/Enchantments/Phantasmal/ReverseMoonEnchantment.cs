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
using CrystalMoon.Systems.MiscellaneousMath;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Phantasmal
{
    internal class ReverseMoonEnchantment: BaseEnchantment
    {
        private bool _init;
        private float _traveledDistance;
        private Vector2 _oldPos;
        public override void AI()
        {
            base.AI();
            if (!_init)
            {
                _oldPos = Projectile.position;
                _init = true;
            }
            if(_oldPos != Projectile.position)
            {
                _traveledDistance += Vector2.Distance(_oldPos, Projectile.position);
                _oldPos = Projectile.position;
            }
        
        }
        public override void SetMagicDefaults()
        {
            Projectile.penetrate += 1;
        }

        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<PhantasmalElement>();
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.PhantasmalGreen);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Projectile.Center += Projectile.velocity.SafeNormalize(Vector2.Zero) * _traveledDistance;
            Projectile.velocity = -Projectile.velocity;
        }
    }
}

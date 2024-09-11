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
using CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Content.Bases;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Uvilis
{
    internal class TsunamiEnchantment : BaseEnchantment
    {
        public override float GetStaffManaModifier()
        {
            return 0.2f;
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
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, Color.Red);
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            /*
            //Spawn the explosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 
                Projectile.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<TsunamiEnchantmentExplosion>(),
              Projectile.damage / 2, Projectile.knockBack, Projectile.owner);*/
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            /*
            //Spawn the explosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center,
                Projectile.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<TsunamiEnchantmentExplosion>(),
                Projectile.damage / 2, Projectile.knockBack, Projectile.owner);*/
        }
    }
}

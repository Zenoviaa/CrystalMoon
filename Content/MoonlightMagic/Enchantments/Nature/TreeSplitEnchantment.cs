using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.ScreenSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Nature
{
    internal class TreeSplitEnchantment : BaseEnchantment
    {
        private int _timer;
        public override void SetDefaults()
        {
            base.SetDefaults();
            time = 60;
        }

        public override void AI()
        {
            base.AI();
            if (!MagicProj.IsClone)
            {
                //Count up
                _timer++;

                //If greater than time then start homing, we'll just swap the movement type of the projectile
                if (_timer == time)
                {
                    int count = 3;
                    for (float i = 0; i < count; i++)
                    {
                        float progress = i / count;
                        float angle = MathHelper.PiOver4;
                        float fireRot = progress * angle;
                        Vector2 fireDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
                        Vector2 firePoint = Projectile.Center;
                        Vector2 fireVelocity = fireDirection.RotatedBy(fireRot - angle / 2f) * Projectile.velocity.Length() * 0.5f;
                        AdvancedMagicUtil.CloneMagicProjectile(MagicProj, firePoint, fireVelocity, Projectile.damage / count, Projectile.knockBack / count,
                            MagicProj.TrailLength / count, MagicProj.Size / count);
                    }

                    Projectile.Kill();
                }
            }      
        }

        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<NaturalElement>();
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.NaturalGreen);
        }
    }
}

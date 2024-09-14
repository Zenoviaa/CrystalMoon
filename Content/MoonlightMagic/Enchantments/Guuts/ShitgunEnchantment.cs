﻿using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.ScreenSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Guuts
{
    internal class ShitgunEnchantment : BaseEnchantment
    {
        
        public override void SetDefaults()
        {
            base.SetDefaults();
          
        }


        public override void AI()
        {
            base.AI();
            if (!MagicProj.IsClone)
            {
                //Count up


                //If greater than time then start homing, we'll just swap the movement type of the projectile

                int count = Main.rand.Next(4,10);
                for (float i = 0; i < count; i++)
                {
                    float progress = i / count;
                    float angle = MathHelper.PiOver4 + Main.rand.NextFloat(0, MathHelper.PiOver4);
                    float fireRot = progress * angle;
                    Vector2 fireDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
                    Vector2 firePoint = Projectile.Center;
                    Vector2 fireVelocity = fireDirection.RotatedBy(fireRot - angle / 2f) * Projectile.velocity.Length();
                    AdvancedMagicUtil.CloneMagicProjectile(MagicProj, firePoint, fireVelocity, Projectile.damage / 3, Projectile.knockBack * 2,
                        MagicProj.TrailLength, MagicProj.Size / count);
                }

                Projectile.Kill();

            }
        }

        public override float GetStaffManaModifier()
        {
            return 0.7f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<GuutElement>();
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.GuutGray);
        }
    }
}

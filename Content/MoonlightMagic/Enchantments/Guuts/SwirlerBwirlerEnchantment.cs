using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Guuts
{
    internal class SwirlerBwirlerEnchantment : BaseEnchantment
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            time = 30;
        }

        public override void AI()
        {
            base.AI();

            //Count up
            Countertimer++;

            //If greater than time then start homing, we'll just swap the movement type of the projectile
            if (Countertimer == time)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 spawnPoint = Projectile.Center + Main.rand.NextVector2Circular(8, 8);
                    Vector2 velocity = Main.rand.NextVector2Circular(8, 8);
                    Particle.NewParticle<GlowParticle>(spawnPoint, velocity, Color.White);
                }

                Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.TwoPi);
            }
        }

        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<GuutElement>();
        }
    }
}

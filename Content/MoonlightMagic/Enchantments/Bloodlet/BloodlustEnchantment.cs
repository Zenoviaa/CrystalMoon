﻿using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Bloodlet
{
    internal class BloodlustEnchantment : BaseEnchantment
    {
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            time = 15;
        }

        public override void AI()
        {
            base.AI();
            Countertimer++;
            if(Countertimer == time)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 spawnPoint = Projectile.Center + Main.rand.NextVector2Circular(8, 8);
                    Vector2 velocity = Main.rand.NextVector2Circular(8, 8);

                    Color color = Color.White;
                    color.A = 0;
                    Particle.NewBlackParticle<BloodSparkleParticle>(spawnPoint, velocity, color);
                }

                Projectile.damage *= 2;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            player.statLife -= 25;
            for (int i = 0; i < 12; i++)
            {
                Vector2 spawnPoint = player.Center + Main.rand.NextVector2Circular(8, 8);
                Vector2 velocity = Main.rand.NextVector2Circular(8, 8);

                Color color = Color.White;
                color.A = 0;
                Particle.NewBlackParticle<BloodSparkleParticle>(spawnPoint, velocity, color);
            }
            CombatText.NewText(player.getRect(), Color.Red, "-25", true);
            return true;
        }

        public override float GetStaffManaModifier()
        {
            return 0.13f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<BloodletElement>();
        }
    }
}

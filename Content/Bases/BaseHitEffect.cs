using CrystalMoon.Systems.Particles;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Bases
{
    public class BaseHitEffect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 192;
            Projectile.height = 192;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 110;
            Projectile.timeLeft = 900;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.4f;
        }

        public override bool PreAI()
        {
            Projectile.ai[0]++;
            Projectile.alpha -= 40;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            if (Projectile.ai[0] <= 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 spawnPoint = Projectile.Center + Main.rand.NextVector2Circular(8, 8);
                    Vector2 velocity = Main.rand.NextVector2Circular(8, 8);
                    Particle.NewParticle<GlowParticle>(spawnPoint, velocity, Color.White);
                }

                /*
                SoundStyle soundStyle = new SoundStyle("Stellamod/Assets/Sounds/RipperSlash2");
                soundStyle.PitchVariance = 0.5f;
                SoundEngine.PlaySound(soundStyle, Projectile.position);
                */
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 7)
                {
                    Projectile.active = false;
                }


            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.ShimmerSplash, 0, 60, 133);
            }

            for (int i = 0; i < 5; i++)
            {
                Vector2 spawnPoint = Projectile.Center + Main.rand.NextVector2Circular(8, 8);
                Vector2 velocity = Main.rand.NextVector2Circular(8, 8);
                Particle.NewParticle<GlowParticle>(spawnPoint, velocity, Color.White);
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;



        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
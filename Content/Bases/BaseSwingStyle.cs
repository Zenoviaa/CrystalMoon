using System;
using Terraria;
using Terraria.Audio;

namespace CrystalMoon.Content.Bases
{
    public abstract class BaseSwingStyle
    {
        public BaseSwingStyle()
        {

        }

        public BaseSwingProjectile SwingProjectile { get; set; }
        public Projectile Projectile => SwingProjectile.Projectile;
        public Player Owner => SwingProjectile.Owner;
        public float swingTime;
        public SoundStyle swingSound;
        public Func<float, float> easingFunc;
        public abstract void AI();
    }
}

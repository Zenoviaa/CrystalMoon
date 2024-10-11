using CrystalMoon.Systems.MiscellaneousMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

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
        public Func<float, float> easingFunc;
        public abstract void AI();
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseForm : IMagicAddon
    {
        public MoonlightMagicProjectile MagicProj { get; set; }
        public Projectile Projectile => MagicProj.Projectile;
        public virtual void PreDraw(ref Color lightColor) { }
        public virtual void PostDraw(Color lightColor) { }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalMoon.Content.Bases.MoonlightMagic
{
    internal abstract class BaseForm
    {
        public virtual void PreDraw(ref Color lightColor) { }
        public virtual void PostDraw(Color lightColor) { }
    }
}

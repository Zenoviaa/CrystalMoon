using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CrystalMoon.Systems.MiscellaneousMath
{
    internal static class MathUtil
    {
        public static float Osc(float from, float to, float speed = 1f, float offset = 0f)
        {
            float dif = (to - from) / 2f;
            return from + dif + dif * (float)Math.Sin(Main.GlobalTimeWrappedHourly * speed + offset);
        }
    }
}

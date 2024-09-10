using CrystalMoon.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CrystalMoon.Content.MoonlightMagic.Movements
{
    internal class CircularMovement : BaseMovement
    {
       // public float maxHomingDetectDistance = 512;
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(15));
        }
    }
}

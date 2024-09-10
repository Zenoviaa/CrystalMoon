using CrystalMoon.Systems.MiscellaneousMath;
using Terraria;
using Terraria.ID;

namespace CrystalMoon.Content.MoonlightMagic.Movements
{
    internal class LobberMovement : BaseMovement
    {
       // public float maxHomingDetectDistance = 512;
        public override void AI()
        {

            Projectile.velocity.Y += 0.05f;
        }
    }
}

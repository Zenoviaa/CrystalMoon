﻿using CrystalMoon.Systems.MiscellaneousMath;
using Terraria;

namespace CrystalMoon.Content.MoonlightMagic.Movements
{
    internal class HomingLongMovement : BaseMovement
    {
        public float maxHomingDetectDistance = 512;
        public override void AI()
        {
            NPC npcToChase = ProjectileHelper.FindNearestEnemy(Projectile.Center, maxHomingDetectDistance);
            if (npcToChase != null)
                Projectile.velocity = ProjectileHelper.SimpleHomingVelocity(Projectile, npcToChase.Center, degreesToRotate: 4);
        }
    }
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CrystalMoon.Content.Bases
{
    public class SpearSwingStyle : BaseSwingStyle
    {
        private bool _thrust;
        public float thrustSpeed;
        public float stabRange;
        public override void AI()
        {
            float lerpValue = SwingProjectile.Countertimer / SwingProjectile.GetSwingTime(swingTime);

            float swingProgress = lerpValue;
            float targetRotation = Projectile.velocity.ToRotation();
            swingProgress = easingFunc(swingProgress);
            SwingProjectile._smoothedLerpValue = swingProgress;
            float dir2 = (int)Projectile.ai[1];

            Vector2 swingDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Vector2 swingVelocity = swingDirection * stabRange;
            if (!_thrust)
            {
                Owner.velocity += swingDirection * thrustSpeed;
                _thrust = true;
            }

            Projectile.Center = Owner.Center +
                Vector2.Lerp(Vector2.Zero, swingVelocity, swingProgress) + swingDirection * SwingProjectile.holdOffset;
            Projectile.rotation = (Projectile.Center - Owner.Center).ToRotation() + MathHelper.PiOver4;

            Vector2[] points = new Vector2[ProjectileID.Sets.TrailCacheLength[Projectile.type]];
            for (int i = 0; i < points.Length; i++)
            {
                float l = points.Length;
                //Lerp between the points
                float progressOnTrail = i / l;

                //Calculate starting lerp value
                float startTrailLerpValue = MathHelper.Clamp(lerpValue - SwingProjectile.trailStartOffset, 0, 1);
                float startTrailProgress = startTrailLerpValue;
                startTrailProgress = easingFunc(startTrailLerpValue);

                //Calculate ending lerp value
                float endTrailLerpValue = lerpValue;
                float endTrailProgress = endTrailLerpValue;
                endTrailProgress = easingFunc(endTrailLerpValue);

                //Lerp in between points
                float smoothedTrailProgress = MathHelper.Lerp(startTrailProgress, endTrailProgress, progressOnTrail);
                Vector2 pos = Owner.Center +
                    Vector2.Lerp(Vector2.Zero, swingVelocity, smoothedTrailProgress) + swingDirection * SwingProjectile.holdOffset;
                points[i] = pos - (SwingProjectile.GetFramingSize() / 2); //+ (pos - Owner.RenderPosition).SafeNormalize(Vector2.Zero) * HoldOffset;// - (GetFramingSize() / 2);// + GetTrailOffset().RotatedBy(targetRotation);
            };

            SwingProjectile._trailPoints = points;
        }
    }
}

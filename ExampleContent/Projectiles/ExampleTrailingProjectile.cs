using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.ExampleContent.Projectiles
{
    internal class ExampleTrailingProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {    
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;     
        }

        public override void AI()
        {
            base.AI();
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 48);
        }

        private Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.Black, p);
        }

        private float WidthFunction(float p)
        {
            return MathHelper.Lerp(250, 0, Easing.OutExpo(p, 6));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;

            //Main trailing texture
            simpleTrailShader.TrailingTexture = TrailRegistry.StarTrail;

            //Blends with the main texture
            simpleTrailShader.SecondaryTrailingTexture = TrailRegistry.StarTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            simpleTrailShader.TertiaryTrailingTexture = TrailRegistry.CrystalTrail;
            simpleTrailShader.PrimaryColor = Color.Red;
            simpleTrailShader.SecondaryColor = Color.Green;

            //Alpha Blend/Additive
            simpleTrailShader.BlendState = BlendState.NonPremultiplied;

            //How many times to draw the trail, useful for additive drawing
            simpleTrailShader.DrawCount = 3;

            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
                Projectile.oldPos, 
                Projectile.oldRot, 
                ColorFunction,
                WidthFunction, simpleTrailShader, offset: new Vector2(Projectile.width / 2, Projectile.height / 2));
            simpleTrailShader.BlendState = BlendState.Additive;
            TrailDrawer.Draw(spriteBatch,
              Projectile.oldPos,
              Projectile.oldRot,
              ColorFunction,
              WidthFunction, simpleTrailShader, offset: new Vector2(Projectile.width / 2, Projectile.height / 2));
            return base.PreDraw(ref lightColor);
        }
    }
}

using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Bases
{
    internal abstract class BaseExplosionProjectile : ModProjectile
    {
        protected Vector2[] _circlePos = new Vector2[64];
        private ref float Countertimer => ref Projectile.ai[0];
        private float _duration;
        private float _beamWidth;
        private Color _beamColor;
        public override string Texture => TextureRegistry.EmptyTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            base.AI();
            Countertimer++;
            if(Countertimer == 1)
            {
                _duration = Projectile.timeLeft;
            }
 
            float progress = Countertimer / _duration;
            float r = RadiusFunction(progress);
            _beamWidth = BeamWidthFunction(progress);
            _beamColor = ColorFunction(progress);

    
            for (int f = 0; f < _circlePos.Length; f++)
            {
                float p = f / (float)_circlePos.Length;
                Vector2 circlePos = Projectile.Center + Vector2.UnitY.RotatedBy(p * MathHelper.TwoPi * 3) * r;
                _circlePos[f] = circlePos;
            }
         
        }

        protected virtual Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.Black, p);
        }

        protected virtual float BeamWidthFunction(float p)
        {
            return _beamWidth;
        }

        protected virtual float WidthFunction(float p)
        {
            return _beamWidth;
        }
        protected Color ColorFunctionReal(float p)
        {
            return _beamColor;
        }
        protected virtual float RadiusFunction(float p)
        {
            return MathHelper.Lerp(16, 64, Easing.OutExpo(p));
        }

        protected virtual BaseShader ReadyShader()
        {
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;

            //Main trailing texture
            simpleTrailShader.TrailingTexture = TextureRegistry.StarTrail;

            //Blends with the main texture
            simpleTrailShader.SecondaryTrailingTexture = TextureRegistry.StarTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            simpleTrailShader.TertiaryTrailingTexture = TextureRegistry.CrystalTrail;
            simpleTrailShader.PrimaryColor = Color.Red;
            simpleTrailShader.SecondaryColor = Color.Green;

            //Alpha Blend/Additive
            simpleTrailShader.BlendState = BlendState.Additive;
            simpleTrailShader.FillShape = true;
            return simpleTrailShader;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            BaseShader shader = ReadyShader();
            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
                _circlePos,
                Projectile.oldRot,
                ColorFunctionReal,
                WidthFunction, shader, offset: Vector2.Zero);
           
            return base.PreDraw(ref lightColor);
        }
    }
}

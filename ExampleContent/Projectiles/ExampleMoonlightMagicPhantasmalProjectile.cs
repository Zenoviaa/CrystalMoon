using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.ExampleContent.Projectiles
{
    internal class ExampleMoonlightMagicPhantasmalProjectile : ModProjectile
    {
        int trailingMode = 0;
        private ref float _timer => ref Projectile.ai[0];
        public override string Texture => TextureRegistry.EmptyTexturePath;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 108;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.light = 0.67f;
        }

        public override void AI()
        {
            base.AI();
            _timer++;
            ProjectileID.Sets.TrailCacheLength[Type] = 36;
            AI_Particles();
            Projectile.velocity = Vector2.Lerp(
                 Projectile.velocity,
                 (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 8, 0.1f);
        }

        private void AI_Particles()
        {

        }

        private Color ColorFunction(float completionRatio)
        {
            return Color.Lerp(new Color(69, 196, 182), Color.SpringGreen, completionRatio);
        }

        private float WidthFunction(float completionRatio)
        {
            float p  = Easing.SpikeOutCirc(completionRatio);
            float ep = Easing.InBack(1-completionRatio);
            return MathHelper.Lerp(0, 108, p * ep);
        }

        private void DrawMainShader()
        {
            trailingMode = 0;
            var shader = MagicPhantasmalShader.Instance;
            shader.PrimaryTexture = TextureRegistry.GlowTrail;
            shader.NoiseTexture = TextureRegistry.SpikyTrail;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.5f;
            shader.Repeats = 1f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, Projectile.oldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Trail
            DrawMainShader();
            return base.PreDraw(ref lightColor);
        }
    }
}

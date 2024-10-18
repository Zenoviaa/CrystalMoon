using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class HolinessElement : BaseElement
    {
        public override Color GetElementColor()
        {
            return Color.LightGray;
        }

        public override bool DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset)
        {
            base.DrawTextShader(spriteBatch, item, line, ref yOffset);
            EnchantmentDrawHelper.DrawTextShader(spriteBatch, item, line, ref yOffset,
                glowColor: new Color(27, 157, 241),
                primaryColor: Color.White,
                noiseColor: Color.Lerp(new Color(27, 157, 241), Color.Black, 0.3f));
            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, Color.White);
        }

        public override void OnKill()
        {
            base.OnKill();
            SpawnDeathParticles();
        }

        private void SpawnDeathParticles()
        {
            //Kill Trail
            for (int i = 0; i < MagicProj.OldPos.Length - 1; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                Vector2 spawnPoint = MagicProj.OldPos[i] + offset + Projectile.Size / 2;
                Vector2 velocity = MagicProj.OldPos[i + 1] - MagicProj.OldPos[i];
                velocity = velocity.SafeNormalize(Vector2.Zero) * -2;

                Color color = Color.White;
                color.A = 0;
                Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity, color);
            }

            for (float f = 0f; f < 1f; f += 0.2f)
            {
                float rot = f * MathHelper.TwoPi;
                Vector2 spawnPoint = Projectile.position;
                Vector2 velocity = rot.ToRotationVector2() * Main.rand.NextFloat(0f, 4f);

                Color color = Color.White;
                color.A = 0;
                Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity, color);
            }
        }

        #region Visuals

        public override void DrawForm(SpriteBatch spriteBatch, Texture2D formTexture, Vector2 drawPos, Color drawColor, Color lightColor, float drawRotation, float drawScale)
        {
            float p = MathUtil.Osc(0f, 1f, speed: 3);
            drawColor = Color.White;
            base.DrawForm(spriteBatch, formTexture, drawPos, drawColor, lightColor, drawRotation, drawScale);
        }

        public override void DrawTrail()
        {
            base.DrawTrail();
            var shader = MagicBloodletShader.Instance;
            shader.PrimaryTexture = TextureRegistry.BloodletTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            shader.PrimaryColor = Color.White;
            shader.NoiseColor = Color.Lerp(Color.White, Color.LightGray, 0.2f);
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 10.5f;
            shader.Distortion = 0.1f;
            shader.Alpha = 0.25f;

            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        private Color ColorFunction(float completionRatio)
        {
            Color c = Color.White;
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            float width = 32 * 1.5f * MagicProj.ScaleMultiplier;
            return MathHelper.Lerp(width, 0, completionRatio);
        }
        #endregion
    }
}

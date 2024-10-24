﻿using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Systems.Shaders.MagicTrails;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class UvilisElement : BaseElement
    {
        int trailingMode = 0;
        public override int GetOppositeElementType()
        {
            return ModContent.ItemType<HexElement>();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            SoundStyle uvilisCastStyle = SoundRegistry.UvilisCast;
            uvilisCastStyle.PitchVariance = 0.15f;
            CastSound = uvilisCastStyle;

            SoundStyle uvilisHitStyle = SoundRegistry.UvilisHit;
            uvilisHitStyle.PitchVariance = 0.15f;
            HitSound = uvilisHitStyle;
        }

        public override bool DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset)
        {
            base.DrawTextShader(spriteBatch, item, line, ref yOffset);
            EnchantmentDrawHelper.DrawTextShader(spriteBatch, item, line, ref yOffset,
                glowColor: ColorUtil.UvilisLightBlue,
                primaryColor: Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f),
                noiseColor: Color.Blue);
            return true;
        }

        public override Color GetElementColor()
        {
            return ColorUtil.UvilisLightBlue;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.UvilisLightBlue);
        }

        public override void AI()
        {
            base.AI();
            AI_Particles();
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

                if (Main.rand.NextBool(2))
                {
                    Color color = Color.White;
                    color.A = 0;
                    Particle.NewBlackParticle<WaterSparkleParticle>(spawnPoint, velocity, color);
                }
                else
                {
                    Color color = ColorUtil.UvilisLightBlue;
                    color.A = 0;
                    Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity, color);
                    Particle.NewBlackParticle<WaterSparkleParticle>(spawnPoint, velocity, new Color(255, 255, 255, 0));
                }
            }

            for (float f = 0f; f < 1f; f += 0.2f)
            {
                float rot = f * MathHelper.TwoPi;
                Vector2 spawnPoint = Projectile.position;
                Vector2 velocity = rot.ToRotationVector2() * Main.rand.NextFloat(0f, 4f);

                if (Main.rand.NextBool(2))
                {
                    Color color = ColorUtil.UvilisLightBlue;
                    color.A = 0;
                    Particle.NewBlackParticle<SparkleUvilisParticle>(spawnPoint, velocity, color);
                }
                else
                {

                    Color color = Color.White;
                    color.A = 0;
                    Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity * 0.2f, color);
                    Particle.NewBlackParticle<WaterSparkleParticle>(spawnPoint, velocity, new Color(255, 255, 255, 0));
                }
            }
        }

        private void AI_Particles()
        {
            if (MagicProj.GlobalTimer % 8 == 0)
            {
                for (int i = 0; i < MagicProj.OldPos.Length - 1; i++)
                {
                    if (!Main.rand.NextBool(2))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = MagicProj.OldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = MagicProj.OldPos[i + 1] - MagicProj.OldPos[i];
                    velocity = velocity.SafeNormalize(Vector2.Zero) * -8;

                    Color color = Color.White;
                    color.A = 0;
                    if (Main.rand.NextBool(7))
                    {
                        Particle.NewBlackParticle<WaterSparkleParticle>(spawnPoint, velocity, color);
                    }

                    if (Main.rand.NextBool(16))
                    {
                        Particle.NewBlackParticle<SparkleUvilisParticle>(spawnPoint, velocity, color);
                    }
                }
            }
        }

        private Color ColorFunction(float completionRatio)
        {
            Color c = Color.Blue;
            switch (trailingMode)
            {
                default:
                case 0:
                    break;
                case 1:
                    c.A = 0;
                    break;
                case 2:
                    c.A = 0;
                    break;
            }

            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            float width = 16 *  1.3f * MagicProj.ScaleMultiplier;
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            switch (trailingMode)
            {
                default:
                case 0:
                    return MathHelper.Lerp(0, width, completionRatio);
                case 1:
                    return MathHelper.Lerp(0, width, completionRatio);
                case 2:
                    return MathHelper.Lerp(0, width + 12, completionRatio);
            }
        }


        public override void DrawForm(SpriteBatch spriteBatch, Texture2D formTexture, Vector2 drawPos, Color drawColor, Color lightColor, float drawRotation, float drawScale)
        {
            var shader = PixelMagicSparkleWaterShader.Instance;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            shader.OutlineTexture = TextureRegistry.DottedTrailOutline;
            shader.PrimaryColor = Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f);
            shader.NoiseColor = new Color(92, 100, 255);
            shader.OutlineColor = Color.Black;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.8f;
            shader.Distortion = 0.5f;
            shader.Power = 0.5f;
            shader.Threshold = 0.25f;
            shader.Apply();

            float p = MathUtil.Osc(0f, 1f, speed: 3);
            drawColor = Color.Lerp(ColorUtil.UvilisLightBlue, Color.LightGoldenrodYellow, p);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, default, shader.Effect, Main.GameViewMatrix.ZoomMatrix);

            base.DrawForm(spriteBatch, formTexture, drawPos, drawColor, lightColor, drawRotation, drawScale * 2f);

            spriteBatch.End();
            spriteBatch.Begin();
            base.DrawForm(spriteBatch, formTexture, drawPos, drawColor, lightColor, drawRotation, drawScale);
        }

        private void DrawMainShader()
        {
            trailingMode = 0;
            var shader = MagicSparkleWaterShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;
            shader.OutlineTexture = TextureRegistry.DottedTrailOutline;
            shader.PrimaryColor = Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f);
            shader.NoiseColor = new Color(92, 100, 255);
            shader.OutlineColor = Color.Black;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.8f;
            shader.Distortion = 0.25f;
            shader.Power = 0.5f;
            shader.Threshold = 0.25f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);

        }

        private void DrawOutlineShader()
        {
            trailingMode = 1;
            var shader = MagicRadianceOutlineShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrailOutline;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;

            Color c = new Color(38, 204, 255);
            shader.PrimaryColor = c;
            shader.NoiseColor = c;
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.8f;
            shader.Distortion = 0.25f;
            shader.Power = 2.5f;

            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        private void DrawOutlineShader2()
        {
            trailingMode = 2;
            var shader = MagicRadianceOutlineShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrailOutline;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;

            Color c = Color.White;
            shader.PrimaryColor = c;
            shader.NoiseColor = c;
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 0.8f;
            shader.Distortion = 0.25f;
            shader.Power = 3.5f;

            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        public override void DrawTrail()
        {
            base.DrawTrail();
            DrawMainShader();
            DrawOutlineShader();
            DrawOutlineShader2();
        }
    }
}

﻿using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders.MagicTrails;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class LightningElement : BaseElement
    {
        private int trailMode = 0;
        private LightningTrail _lightningTrail;
        public override void SetDefaults()
        {
            base.SetDefaults();
            SoundStyle castStyle = SoundID.DD2_LightningAuraZap;
            castStyle.PitchVariance = 0.15f;
            CastSound = castStyle;

            SoundStyle hitStyle = SoundID.DD2_LightningBugZap;
            hitStyle.PitchVariance = 0.15f;
            HitSound = hitStyle;
        }

        public override Color GetElementColor()
        {
            return new Color(120, 215, 255);
        }

        public override bool DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset)
        {
            base.DrawTextShader(spriteBatch, item, line, ref yOffset);
            EnchantmentDrawHelper.DrawTextShader(spriteBatch, item, line, ref yOffset,
                glowColor: Color.Yellow,
                primaryColor: Color.Lerp(Color.White, Color.Yellow, 0.5f),
                noiseColor: new Color(120, 215, 255));
            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, Color.Yellow);
        }


        public override void AI()
        {
            AI_Particles();
        }

        private void AI_Particles()
        {
            if (MagicProj.GlobalTimer % 3 == 0)
            {
                _lightningTrail ??= new();
                _lightningTrail.RandomPositions(MagicProj.OldPos);
            }

            if (MagicProj.GlobalTimer % 8 == 0)
            {
                for (int i = 0; i < MagicProj.OldPos.Length - 1; i++)
                {
                    if (!Main.rand.NextBool(4))
                        continue;
                    Vector2 offset = Main.rand.NextVector2Circular(16, 16);
                    Vector2 spawnPoint = MagicProj.OldPos[i] + offset + Projectile.Size / 2;
                    Vector2 velocity = MagicProj.OldPos[i + 1] - MagicProj.OldPos[i];
                    velocity = velocity.SafeNormalize(Vector2.Zero) * -2;

                    Color color = Color.Yellow;
                    if (Main.rand.NextBool(2))
                        color = new Color(120, 215, 255);
                    color.A = 0;
                    Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity, color, Scale: 0.5f);
                }
            }
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

                Color color = Color.Yellow;
                if (Main.rand.NextBool(2))
                    color = new Color(120, 215, 255);
                color.A = 0;
                Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity * 0.2f, color);
            }

            for (float f = 0f; f < 1f; f += 0.2f)
            {
                float rot = f * MathHelper.TwoPi;
                Vector2 spawnPoint = Projectile.position;
                Vector2 velocity = rot.ToRotationVector2() * Main.rand.NextFloat(0f, 4f);

                Color color = Color.Yellow;
                if (Main.rand.NextBool(2))
                    color = new Color(120, 215, 255);
                color.A = 0;
                Particle.NewBlackParticle<GlowParticle>(spawnPoint, velocity * 0.2f, color);
            }
        }

        #region Visuals
        public override void DrawForm(SpriteBatch spriteBatch, Texture2D formTexture, Vector2 drawPos, Color drawColor, Color lightColor, float drawRotation, float drawScale)
        {
            base.DrawForm(spriteBatch, formTexture, drawPos, drawColor, lightColor, drawRotation, drawScale);
        }

        public override void DrawTrail()
        {
            //Trail
            SpriteBatch spriteBatch = Main.spriteBatch;
            LightningBoltShader lightningShader = LightningBoltShader.Instance;
            lightningShader.PrimaryColor = Color.Yellow;
            lightningShader.NoiseColor = new Color(120, 215, 255);
            lightningShader.Speed = 5;

            _lightningTrail ??= new();
            //Making this number big made like the field wide
            _lightningTrail.LightningRandomOffsetRange = 5;

            //This number makes it more lightning like, lower this is the straighter it is
            _lightningTrail.LightningRandomExpand = 24;
            _lightningTrail.Draw(spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, lightningShader, offset: Projectile.Size / 2f);
        }

        private float WidthFunction(float completionRatio)
        {
            float progress = completionRatio / 0.3f;
            float rounded = Easing.SpikeOutCirc(progress);
            float spikeProgress = Easing.SpikeOutExpo(completionRatio);
            float fireball = MathHelper.Lerp(rounded, spikeProgress, Easing.OutExpo(1.0f - completionRatio));
            float midWidth = 28 * MagicProj.ScaleMultiplier;
            return MathHelper.Lerp(0, midWidth, fireball);
        }

        private Color ColorFunction(float p)
        {
            Color trailColor = Color.Lerp(Color.White, Color.Yellow, p);
            return trailColor;
        }
        #endregion
    }
}

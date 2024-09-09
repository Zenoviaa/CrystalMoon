using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using CrystalMoon.Systems.ScreenSystems;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class UvilisElement : BaseElement
    {
        int trailingMode = 0;
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
            float width = MagicProj.Size *  1.3f;
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

using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class RadianceElement : BaseElement
    {
        private int trailMode = 0;
        
        public override Color GetElementColor()
        {
            return ColorUtil.RadianceYellow;
        }

        public override bool DrawTextShader(SpriteBatch spriteBatch, Item item, DrawableTooltipLine line, ref int yOffset)
        {
            base.DrawTextShader(spriteBatch, item, line, ref yOffset);
            Vector2 textPosition = new Vector2(line.X, line.Y);
            //Draw BackGlow
            var glowTexture = TextureRegistry.GlowTrail;
            Vector2 scale = new Vector2(0.45f, 0.15f);
            Color glowColor = Color.OrangeRed;
            glowColor.A = 0;
            glowColor *= 0.5f;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, default, Main.UIScaleMatrix);
            spriteBatch.Draw(glowTexture.Value, textPosition, null, glowColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);


            //Draw Flaming Text
            ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, line.Text, textPosition, line.Color, line.Rotation, line.Origin, line.BaseScale);
            var shader = FirePixelShader.Instance;
            shader.PrimaryColor = Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f);
            shader.NoiseColor = new Color(206, 101, 0);
            shader.Distortion = 0.0075f;
            shader.Speed = 10;
            shader.Power = 0.01f;
            shader.Apply();


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, default, Main.UIScaleMatrix);

            shader.Data.Apply(null);
            ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, line.Text, textPosition, line.Color, line.Rotation, line.Origin, line.BaseScale);

            spriteBatch.End();
            spriteBatch.Begin(default, default, default, default, default, default, Main.UIScaleMatrix);
            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.RadianceYellow);
        }

        public override void AI()
        {
            AI_Particles();
        }

        public override void DrawTrail()
        {
            DrawMainShader();
            DrawOutlineShader();
        }

        private void AI_Particles()
        {
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

                    if (Main.rand.NextBool(2))
                    {
                        Color color = Color.RosyBrown;
                        color.A = 0;
                        Particle.NewBlackParticle<FireSmokeParticle>(spawnPoint, velocity, color);
                    }
                    else
                    {
                        Particle.NewBlackParticle<FireHeatParticle>(spawnPoint, velocity, new Color(255, 255, 255, 0));
                    }
                }
            }
        }

        private Color ColorFunction(float completionRatio)
        {
            Color c;
            switch (trailMode)
            {
                default:
                case 0:
                    c = Color.Lerp(Color.White, new Color(147, 72, 11) * 0.5f, completionRatio);
                    break;
                case 1:
                    c = Color.Lerp(Color.White, new Color(147, 72, 11) * 0f, completionRatio);
                    break;
                case 2:
                    c = Color.White;
                    c.A = 0;
                    break;
            }
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            float progress = completionRatio / 0.3f;
            float rounded = Easing.SpikeOutCirc(progress);
            float spikeProgress = Easing.SpikeOutExpo(completionRatio);
            float fireball = MathHelper.Lerp(rounded, spikeProgress, Easing.OutExpo(1.0f - completionRatio));

            float midWidth = 46;
            switch (trailMode)
            {
                default:
                case 0:
                    return MathHelper.Lerp(0, midWidth, fireball);
                case 1:
                    return MathHelper.Lerp(midWidth / 1.5f / 2f, midWidth / 1.5f, spikeProgress);
                case 2:
                    return MathHelper.Lerp(0, midWidth + 8, fireball);
            }
        }

        private void DrawMainShader()
        {
            //Trail
            trailMode = 0;
            var shader = MagicRadianceShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;
            shader.OutlineTexture = TextureRegistry.DottedTrailOutline;
            shader.PrimaryColor = Color.Lerp(Color.White, new Color(255, 207, 79), 0.5f);
            shader.NoiseColor = new Color(206, 101, 0);
            shader.OutlineColor = Color.Black;
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 5.2f;
            shader.Distortion = 0.15f;
            shader.Power = 0.25f;

            //This just applis the shader changes

            //Main Fill
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);

            //Secondary fill
            trailMode = 0;
            shader.PrimaryColor = new Color(206, 101, 0);
            shader.NoiseColor = Color.Red;
            shader.OutlineColor = Color.Black;
            shader.Speed = 2.2f;
            shader.Distortion = 0.3f;
            shader.Power = 1.5f;
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        private void DrawOutlineShader()
        {
            trailMode = 2;
            var shader = MagicRadianceOutlineShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrailOutline;
            shader.NoiseTexture = TextureRegistry.NoiseTextureCloudsSmall;

            Color c = Color.DarkRed;
            shader.PrimaryColor = c;
            shader.NoiseColor = Color.DarkRed;
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 5.2f;
            shader.Distortion = 0.15f;
            shader.Power = 0.05f;
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }
    }
}

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

namespace CrystalMoon.Content.MoonlightMagic.Elements
{
    internal class GuutElement : BaseElement
    {
        public override Color GetElementColor()
        {
            return ColorUtil.GuutGray;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.GuutGray);
        }

        public override void AI()
        {
            base.AI();
            AI_Particles();
        }

        private void AI_Particles()
        {
            return;
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
            Color c = Color.White;
            return c;
        }

        private float WidthFunction(float completionRatio)
        {
            float width = MagicProj.Size * 1.5f;
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            return MathHelper.Lerp(0, width, completionRatio);
        }


        private void DrawMainShader()
        {
            //Trail
            var shader = MagicGuutShader.Instance;
            shader.PrimaryTexture = TextureRegistry.DottedTrail;
            shader.NoiseTexture = TextureRegistry.CrystalTrail;
            shader.OutlineTexture = TextureRegistry.DottedTrailOutline;
            shader.PrimaryColor = Color.White;
            shader.NoiseColor = Color.Black;
            shader.OutlineColor = Color.Lerp(Color.Black, Color.DarkGray, 0.3f);
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 2.2f;
            shader.Distortion = 0.05f;
            shader.Power = 0.1f;

            //This just applis the shader changes

            //Main Fill
            TrailDrawer.Draw(Main.spriteBatch, MagicProj.OldPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);

        }

        public override void DrawTrail()
        {
            base.DrawTrail();
            DrawMainShader();
        }
    }
}

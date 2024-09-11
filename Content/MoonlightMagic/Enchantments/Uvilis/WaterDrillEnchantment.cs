﻿using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Systems.ScreenSystems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CrystalMoon.Registries;
using CrystalMoon.Content.MoonlightMagic.Movements;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Visual.Particles;
using CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Content.Bases;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Uvilis
{
    internal class WaterDrillEnchantment : BaseEnchantment
    {
        public override float GetStaffManaModifier()
        {
            return 0.4f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<UvilisElement>();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, Color.Red);
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Spawn the explosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                Projectile.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<WaterDrillEnchantmentExplosion>(),
              Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            //Spawn the explosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center,
                Projectile.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<WaterDrillEnchantmentExplosion>(),
                Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
        }
    }

    internal class WaterDrillEnchantmentExplosion : BasePillarExplosionProjectile
    {
        int trailingMode = 0;
        protected override float DistanceFunction(float completionRatio)
        {
            return 300 * (1.0f - Easing.SpikeOutExpo(completionRatio)) * GlobalWidth;
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

        //This scales off of the projectile's lifetime rathr  than trail length
        protected override float GlobalWidthFunction(float completionRatio)
        {
            return Easing.SpikeOutCirc(completionRatio);
        }

        private float WidthFunction(float completionRatio)
        {
            float width = 48 * 1.3f;
            completionRatio = Easing.SpikeOutCirc(completionRatio);
            switch (trailingMode)
            {
                default:
                case 0:
                    return MathHelper.Lerp(0, width, completionRatio) * GlobalWidth;
                case 1:
                    return MathHelper.Lerp(0, width, completionRatio) * GlobalWidth;
                case 2:
                    return MathHelper.Lerp(0, width + 12, completionRatio) * GlobalWidth;
            }
        }


        private void DrawMainShader(Vector2[] trailPos)
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
            shader.Speed = 10.8f;
            shader.Distortion = 0.25f;
            shader.Power = 0.5f;
            shader.Threshold = 0.25f;
            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, trailPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        private void DrawOutlineShader(Vector2[] trailPos)
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
            shader.Speed = 10.8f;
            shader.Distortion = 0.25f;
            shader.Power = 2.5f;

            TrailDrawer.Draw(Main.spriteBatch, trailPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

        private void DrawOutlineShader2(Vector2[] trailPos)
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
            shader.Speed = 10.8f;
            shader.Distortion = 0.25f;
            shader.Power = 3.5f;

            TrailDrawer.Draw(Main.spriteBatch, trailPos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }


        protected override void DrawPrims(Vector2[] trailPos)
        {
            base.DrawPrims(trailPos);
            DrawMainShader(trailPos);
            DrawOutlineShader(trailPos);
            DrawOutlineShader2(trailPos);
        }
    }

}

﻿using CrystalMoon.Content.Bases;
using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Bloodlet
{
    internal class CoagulateEnchantment : BaseEnchantment
    {
        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<BloodletElement>();
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Spawn the explosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CoagulateEnchantmentExplosion>(),
              Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            //Spawn the explosion
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<CoagulateEnchantmentExplosion>(),
                Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
        }
    }

    internal class CoagulateEnchantmentExplosion : BaseSpikeAuraExplosionProjectile
    {
        int rStart = 4;
        int rEnd = 128;
        public override void SetDefaults()
        {
            base.SetDefaults();
            _primPos = new Vector2[16];
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 32;
            rStart = Main.rand.Next(4, 8);
            rEnd = Main.rand.Next(96, 128);
        }

        protected override Color BeamColorFunction(float completionRatio)
        {
            Color c = Color.Red;
            return c;
        }

        protected override float BeamWidthFunction(float completionRatio)
        {

            float trailWidth = MathHelper.Lerp(32, 16, Progress);
            float fadeWidth = MathHelper.Lerp(0, trailWidth,
                Easing.SpikeOutCirc(Progress)) * Main.rand.NextFloat(0.75f, 1.0f);
            return fadeWidth;
        }

        protected override float RadiusFunction(float completionRatio)
        {


            return MathHelper.Lerp(rStart, rEnd, Easing.SpikeOutCirc(completionRatio));
        }

        protected override void DrawPrims(Vector2[] trailPos)
        {
            base.DrawPrims(trailPos);
            DrawMainShader(trailPos);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Vector2 upwardVelocity = -Vector2.UnitY * Projectile.knockBack * 2.5f;
            upwardVelocity *= target.knockBackResist;
            target.velocity += upwardVelocity;
        }

        private void DrawMainShader(Vector2[] trailPos)
        {
            //Trail
            var shader = MagicBloodletShader.Instance;
            shader.PrimaryTexture = TextureRegistry.BloodletTrail;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            shader.PrimaryColor = new Color(255, 51, 51);
            shader.NoiseColor = Color.Lerp(shader.PrimaryColor, Color.Black, 0.5f);
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.Speed = 10.5f;
            shader.Distortion = 0.1f;
            shader.Alpha = 0.25f;

            //This just applis the shader changes
            TrailDrawer.Draw(Main.spriteBatch, trailPos, Projectile.oldRot,
                BeamColorFunction, BeamWidthFunction, shader, offset: Projectile.Size / 2);
        }
    }
}

﻿using CrystalMoon.Content.Bases;
using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Content.MoonlightMagic.Enchantments.Nature;
using CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance;
using CrystalMoon.Content.MoonlightMagic.Movements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Guuts
{
    internal class MetalForceEnchantment : BaseEnchantment
    {
        public override float GetStaffManaModifier()
        {
            return 0.1f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Spawn the explosion
            for (int i = 0; i < 4; i++)
            {
                Vector2 spawnPoint = Projectile.Center + Main.rand.NextVector2Circular(8, 8);
                Vector2 velocity = Main.rand.NextVector2Circular(8, 8);


                // Particle.NewParticle<SparkleHexParticle>(spawnPoint, velocity, Color.White);

            }


            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0, ModContent.ProjectileType<MetalForceEnchantmentExplosion>(),
               Projectile.damage, Projectile.knockBack, Projectile.owner);

            return true;
        }


        public override int GetElementType()
        {
            return ModContent.ItemType<GuutElement>();
        }


        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

            return true;
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, Color.LightPink);
        }

      
    }

    internal class MetalForceEnchantmentExplosion : BaseExplosionProjectile
    {
        int trailMode;
        int rStart = 4;
        int rEnd = 428;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 60;
            rStart = Main.rand.Next(60 / 2, 64 / 2);
            rEnd = Main.rand.Next(120 / 2, 120 / 2);
        }
       

        public override void OnKill(int timeLeft)
        {
           

            /*
            for (int i = 0; i < 50; i++)
            {
                Vector2 spawnPoint = Projectile.Center + Main.rand.NextVector2Circular(8, 8);
                Vector2 speed = Main.rand.NextVector2CircularEdge(4f, 4f);
                Particle.NewParticle<SparkleHexParticle>(spawnPoint, speed, Color.White);
            }
            */
        }

        protected override float BeamWidthFunction(float p)
        {

            //How wide the trail is going to be
            float trailWidth = MathHelper.Lerp(125 / 2, 13 / 2, p);
            float fadeWidth = MathHelper.Lerp(0, trailWidth, Easing.OutExpo(p)) * Main.rand.NextFloat(0.85f, 1.0f);
            return fadeWidth;
        }

        protected override Color ColorFunction(float p)
        {
            //Main color of the beam
            Color c;
            switch (trailMode)
            {
                default:
                case 0:
                    c = Color.Lerp(Color.White, new Color(147, 72, 121) * 0.5f, p);
                    break;
                case 1:
                    c = Color.Lerp(Color.White, new Color(147, 72, 121) * 0f, p);
                    break;
                case 2:
                    c = Color.White;
                    c.A = 0;
                    break;
            }

            return c;
        }

        protected override float RadiusFunction(float p)
        {
            //How large the circle is going to be
            return MathHelper.Lerp(rStart, rEnd, Easing.InQuint(p));
        }

        protected override BaseShader ReadyShader()
        {
            var shader = SimpleTrailShader.Instance;

            //Main trailing texture
            shader.TrailingTexture = TextureRegistry.GlowTrail;

            //Blends with the main texture
            shader.SecondaryTrailingTexture = TextureRegistry.GlowTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.TertiaryTrailingTexture = TextureRegistry.CrystalTrail;
            shader.PrimaryColor = Color.DarkGoldenrod;
            shader.SecondaryColor = Color.Purple;
            shader.Speed = 20;

            //Alpha Blend/Additive
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.FillShape = true;
            return shader;
        }

        private void DrawMainShader()
        {
            //Trail
            trailMode = 0;
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
            TrailDrawer.Draw(Main.spriteBatch, _circlePos, Projectile.oldRot, ColorFunction, WidthFunction, shader, offset: Projectile.Size / 2);
        }

       

        public override bool PreDraw(ref Color lightColor)
        {
            DrawMainShader();
            // DrawOutlineShader();
            return false ;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
          //  target.AddBuff(BuffID.OnFire, 90);
        }
    }
}

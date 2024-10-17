﻿using CrystalMoon.Content.Bases;
using CrystalMoon.Registries;
using CrystalMoon.Systems;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Players;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Explosions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CrystalMoon.Content.Items.Weapons.Melee.Greatswords
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class Zhielhander : BaseSwingItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CrystalMoon.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTime = 126;
            Item.useAnimation = 126;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<ZhielhanderSwordSlash>();
            Item.autoReuse = true;

            //Combo variables
            //Set combo wait time
            comboWaitTime = 120;
            //Set max combo
            maxCombo = 8;





            //Set stamina to use
            staminaToUse = 1;
            //set staminacombo
            maxStaminaCombo = 1;
            //Set stamina projectile
            staminaProjectileShoot = ModContent.ProjectileType<ZhielhanderStaminaSlash>();
        }


        public override void ShootSwingStamina(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            base.ShootSwingStamina(player, source, position, velocity, type, damage, knockback);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<BasicStaminaExplosion>(), damage, knockback);
        }
    }

    public class ZhielhanderSwordSlash : BaseSwingProjectile
    {
        private NPCSucker _npcSucker;
        public override string Texture => "CrystalMoon/Content/Items/Weapons/Melee/Greatswords/Zhielhander";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 64;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            holdOffset = 40;
            trailStartOffset = 0.2f;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 38 * 2;
            Projectile.width = 38 * 2;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
        }

        public override void SetComboDefaults(List<BaseSwingStyle> swings)
        {
            base.SetComboDefaults(swings);

            SoundStyle swingSound1 = SoundRegistry.HeavySwordSlash1;
            swingSound1.PitchVariance = 0.5f;

            SoundStyle swingSound2 = SoundRegistry.HeavySwordSlash2;
            swingSound2.PitchVariance = 0.5f;

            SoundStyle swingSound3 = SoundRegistry.NSwordSpin1;
            swingSound3.PitchVariance = 0.5f;

            swings.Add(new CircleSwingStyle
            {
                swingDistance = 24,
                swingTime = 48,
                startSwingRotOffset = -MathHelper.ToRadians(175),
                endSwingRotOffset = MathHelper.ToRadians(175),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
                swingSound = swingSound1,
                swingSoundLerpValue = 0.5f
            });

            swings.Add(new SpearSwingStyle
            {
                swingTime = 90,
                stabRange = 240,
                thrustSpeed = 1,
                easingFunc = (float lerpValue) => Easing.SpikeOutExpo(lerpValue),
                swingSound = swingSound3,
                spinRotationRange = MathHelper.ToRadians(2000),
                spinTrailOffset = 1.15f
            });

            swings.Add(new OvalSwingStyle
            {
                swingTime = 70,
                swingXRadius = 84 / 1.5f,
                swingYRadius = 70 / 1.5f,
                swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4,
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
                swingSound = swingSound2,
                swingSoundLerpValue = 0.5f
            });

            swings.Add(new OvalSwingStyle
            {
                swingTime = 70,
                swingXRadius = 168 / 1.5f,
                swingYRadius = 140 / 1.5f,
                swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4,
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
                swingSound = swingSound2,
                swingSoundLerpValue = 0.5f
            });

            swings.Add(new CircleSwingStyle
            {
                swingDistance = 24,
                swingTime = 48,
                startSwingRotOffset = -MathHelper.ToRadians(175),
                endSwingRotOffset = MathHelper.ToRadians(175),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
                swingSound = swingSound1,
                swingSoundLerpValue = 0.5f
            });

            swings.Add(new CircleSwingStyle
            {
                swingDistance = 32,
                swingTime = 96,
                startSwingRotOffset = -MathHelper.ToRadians(215),
                endSwingRotOffset = MathHelper.ToRadians(215),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
                swingSound = swingSound1,
                swingSoundLerpValue = 0.5f
            });
         
            swings.Add(new SpearSwingStyle
            {
                swingTime = 100,
                stabRange = 300,
                thrustSpeed = 2,
                easingFunc = (float lerpValue) => Easing.SpikeOutExpo(lerpValue),
                swingSound = swingSound3,
                spinRotationRange = MathHelper.ToRadians(2000),
                spinTrailOffset = 1.15f
            });

         
            swings.Add(new OvalSwingStyle
            {
                swingTime = 115,
                swingXRadius = 108 / 1.5f,
                swingYRadius = 80 / 1.5f,
                swingRange = MathHelper.ToRadians(1440),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 7),
                swingSound = swingSound3,
                swingSoundLerpValue = 0.15f
            });
        }

        public override bool? CanDamage()
        {
            return true;
        }
        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            if(ComboAtt == 1 || ComboAtt == 6)
            {
                Projectile.knockBack = 0;
                Projectile.localNPCHitCooldown = 12 * ExtraUpdateMult;
            }

            if (ComboAtt == 7)
            {
                //This npc local hit cooldown time makes it hit multiple times
                Projectile.localNPCHitCooldown = 3 * ExtraUpdateMult;
            }
        }

        public override void AI()
        {
            base.AI();
            _npcSucker ??= new NPCSucker();
            if(Countertimer % (ExtraUpdateMult) == 0 && uneasedLerpValue > 0.5f)
            {
                _npcSucker.AI(Projectile.Center, strength: 0.8f);
            } 
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (!Hit)
            {
                CrystalMoonFXUtil.ShakeCamera(target.Center, 1024, 8f);
                Hit = true;
                hitstopTimer = 4 * ExtraUpdateMult;
            }

            if((ComboAtt == 1 || ComboAtt == 6) && uneasedLerpValue > 0.5f)
            {
                _npcSucker.AddNPCSuckerTarget(Projectile.Center, target);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            SoundStyle spearHit = SoundRegistry.SpearHit1;
            spearHit.PitchVariance = 0.5f;
            SoundEngine.PlaySound(spearHit, Projectile.position);
           
            if (ComboAtt == 0)
            {
                modifiers.Knockback *= 2;
            }

            if (ComboAtt == 2)
            {
                modifiers.Knockback *= 2;
            }


            if (ComboAtt == 1 || ComboAtt == 6)
            {
                modifiers.Knockback *= 0;
                modifiers.FinalDamage *= 0.5f;
            }

            if (ComboAtt == 4)
            {
                modifiers.Knockback *= 2;
            }

            if (ComboAtt == 5)
            {
                modifiers.Knockback *= 3;
            }

            if (ComboAtt == 7 && ComboAtt != 6)
            {
                modifiers.FinalDamage *= 2;
            }

        }

        //TRAIL VISUALS
        #region Visuals
        public override Vector2 GetFramingSize()
        {
            //Set this to the width and height of the sword sprite
            return new Vector2(68, 72);
        }

        public override Vector2 GetTrailOffset()
        {
            //Moves the trail along the blade, negative goes towards the player, positive goes away the player
            return Vector2.One * 100;
        }

        protected override float WidthFunction(float p)
        {
            if(ComboAtt == 1 || ComboAtt == 6)
            {
                float spinTrailWidth = MathHelper.Lerp(0, 128, p);
                float spinFadeWidth = MathHelper.Lerp(0, spinTrailWidth, _smoothedLerpValue);
                return spinFadeWidth;
            }
            float trailWidth = MathHelper.Lerp(0, 284, p);
            float fadeWidth = MathHelper.Lerp(trailWidth, 0, _smoothedLerpValue) * Easing.OutExpo(_smoothedLerpValue, 4);
            return fadeWidth;
        }

        protected override Color ColorFunction(float p)
        {
            Color trailColor = Color.Lerp(Color.White, Color.DarkGray, p);
            Color fadeColor = Color.Lerp(trailColor, Color.DarkGray, _smoothedLerpValue);
            //This will make it fade out near the end
            return fadeColor;
        }

        protected override BaseShader ReadyShader()
        {

            var shader = SimpleTrailShader.Instance;
            if(ComboAtt == 1 || ComboAtt == 6)
            {
                //Main trailing texture
                shader.TrailingTexture = TextureRegistry.GlowTrail;

                //Blends with the main texture
                shader.SecondaryTrailingTexture = TextureRegistry.WhispTrail;

                //Used for blending the trail colors
                //Set it to any noise texture
                shader.TertiaryTrailingTexture = TextureRegistry.CausticTrail;
                shader.PrimaryColor = Color.White;
                shader.SecondaryColor = Color.DarkSlateBlue;
                shader.BlendState = BlendState.Additive;
                shader.Speed = 25;
            }
            else
            {
                //Main trailing texture
                shader.TrailingTexture = TextureRegistry.GlowTrail;

                //Blends with the main texture
                shader.SecondaryTrailingTexture = TextureRegistry.WhispTrail;

                //Used for blending the trail colors
                //Set it to any noise texture
                shader.TertiaryTrailingTexture = TextureRegistry.CausticTrail;
                shader.PrimaryColor = Color.White;
                shader.SecondaryColor = Color.DarkSlateBlue;
                shader.BlendState = BlendState.Additive;
                shader.Speed = 25;
            }
        
            return shader;
        }
        #endregion
    }
    public class ZhielhanderStaminaSlash : BaseSwingProjectile
    {
        public override string Texture => "CrystalMoon/Content/Items/Weapons/Melee/Greatswords/Zhielhander";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            holdOffset = -10;
            trailStartOffset = 0.2f;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 38;
            Projectile.width = 38;
            Projectile.friendly = true;
            Projectile.scale = 1.4f;

            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
        }

        private bool _thrust;
        public float thrustSpeed = 9;
        public float stabRange;
        public override void AI()
        {
            base.AI();

            Vector2 swingDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
              if (_smoothedLerpValue > 0.5f)
            {
                if (!_thrust)
                {
                    Owner.velocity += swingDirection * thrustSpeed;
                  
                    Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.Zero);
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Owner.Center, swingDirection * 17, ModContent.ProjectileType<ZhielhanderProj>(), Projectile.damage * 2, 0f, Projectile.owner, 0f, 0f);
                    p.rotation = direction.ToRotation();

                    _thrust = true;

                } 
            }
           


        }
        public override void SetComboDefaults(List<BaseSwingStyle> swings)
        {

            SoundStyle swingSound1 = SoundRegistry.HeavySwordSlash1;
            swingSound1.PitchVariance = 0.5f;


            base.SetComboDefaults(swings);
            swings.Add(new CircleSwingStyle
            {
                swingDistance = 90,
                swingTime = 108,
                startSwingRotOffset = -MathHelper.ToRadians(275),
                endSwingRotOffset = MathHelper.ToRadians(105),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
                swingSound = swingSound1,
                swingSoundLerpValue = 0.5f
            });



        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (!Hit)
            {
                CrystalMoonFXUtil.ShakeCamera(target.Center, 1024, 8f);
                Hit = true;
                hitstopTimer = 8 * ExtraUpdateMult;
            }

        
         }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
          
            SoundStyle spearHit2 = SoundRegistry.NSwordHit1;
            spearHit2.PitchVariance = 0.5f;
            SoundEngine.PlaySound(spearHit2, Projectile.position);

            modifiers.FinalDamage *= 3.5f;
            modifiers.Knockback *= 6;
            
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            ComboPlayer comboPlayer = Owner.GetModPlayer<ComboPlayer>();
            int combo = (int)(ComboAtt + 1);
            int dir = comboPlayer.ComboDirection;


            if (ComboAtt < 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack,
                            Owner.whoAmI, combo, dir);
            }


        }
        //TRAIL VISUALS
        #region Visuals
        public override Vector2 GetFramingSize()
        {
            //Set this to the width and height of the sword sprite
            return new Vector2(68, 72);
        }

        public override Vector2 GetTrailOffset()
        {
            //Moves the trail along the blade, negative goes towards the player, positive goes away the player
            return Vector2.One * 80;
        }

        protected override float WidthFunction(float p)
        {
            float trailWidth = MathHelper.Lerp(0, 584, p);
            float fadeWidth = MathHelper.Lerp(trailWidth, 0, _smoothedLerpValue) * Easing.OutExpo(_smoothedLerpValue, 4);
            return fadeWidth;
        }

        protected override Color ColorFunction(float p)
        {
            Color trailColor = Color.Lerp(Color.White, Color.Black, p);
            Color fadeColor = Color.Lerp(trailColor, Color.Gray, _smoothedLerpValue);
            //This will make it fade out near the end
            return fadeColor;
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
            shader.TertiaryTrailingTexture = TextureRegistry.CrystalTrail2;
            shader.PrimaryColor = Color.White;
            shader.SecondaryColor = Color.DarkGray;
            shader.BlendState = BlendState.Additive;
            shader.Speed = 30;
            return shader;
        }
        #endregion
    }



        internal class ZhielhanderProj : ModProjectile
        {
            bool Moved;

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Frost Wave");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            }
            public override void SetDefaults()
            {
                base.Projectile.width = 41 * 2;
                base.Projectile.height = 41 * 2;
                base.Projectile.timeLeft = 90;
                base.Projectile.scale = 0.7f;
                base.Projectile.friendly = true;
                base.Projectile.hostile = false;
                base.Projectile.ignoreWater = true;
                base.Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            }
            public override void AI()
            {
                Projectile.velocity *= .98f;
            Projectile.scale *= 0.99f;
                Projectile.ai[1]++;
                if (!Moved && Projectile.ai[1] >= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, Projectile.position);
                 //   SoundEngine.PlaySound(new SoundStyle("Stellamod/Assets/Sounds/Astalaiya2"), Projectile.position);
                    Projectile.spriteDirection = Projectile.direction;
                    Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;
                    for (int j = 0; j < 10; j++)
                    {
                        Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
                        vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
                        vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default(Vector2));
                        int num8 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.WhiteTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                        Main.dust[num8].scale = 1.3f;
                        Main.dust[num8].noGravity = true;
                        Main.dust[num8].position = Projectile.Center + vector2;
                        Main.dust[num8].velocity = Projectile.velocity * 0.1f;
                        Main.dust[num8].noLight = true;
                        Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    }
                    Moved = true;
                }

                if (Projectile.ai[1] >= 8)
                {
                    Projectile.ai[1] = 0;
                    Projectile.damage -= 1;
                }


                Projectile.spriteDirection = Projectile.direction;
                Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;
            }
            public override void OnKill(int timeLeft)
            {
                for (int i = 0; i < 20; i++)
                {
                  //  Main.LocalPlayer.GetModPlayer<MyPlayer>().ShakeAtPosition(base.Projectile.Center, 512f, 32f);
                    SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.position);
                    int num1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, 0f, -2f, 0, default(Color), .8f);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num1].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    if (Main.dust[num1].position != Projectile.Center)
                        Main.dust[num1].velocity = Projectile.DirectionTo(Main.dust[num1].position) * 6f;
                    int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, 0f, -2f, 0, default(Color), .8f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    if (Main.dust[num].position != Projectile.Center)
                        Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
                }

            }

            public override bool PreDraw(ref Color lightColor)
            {
                if (Main.rand.NextBool(5))
                {
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 150, Color.MediumPurple, 1f);
                    Main.dust[dustnumber].velocity *= 0.3f;
                    Main.dust[dustnumber].noGravity = true;
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(Color.Lerp(new Color(200, 255, 233), new Color(135, 255, 220), 1f / Projectile.oldPos.Length * k) * (1f - 1f / Projectile.oldPos.Length * k));
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                return false;
            }
            public override void PostDraw(Color lightColor)
            {
                Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.75f * Main.essScale);

            }
        }

    }



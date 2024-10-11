using CrystalMoon.Content.Bases;
using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Players;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Items.Weapons.Melee.Spears
{
    public class CrystalPointer : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CrystalMoon.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<CrystalPointerStab>();
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 60;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);
            comboPlayer.IncreaseCombo(maxCombo: 7);
            return false;
        }
    }

    public class CrystalPointerStab : BaseSwingProjectile
    {
        public override string Texture => "CrystalMoon/Content/Items/Weapons/Melee/Spears/CrystalPointer";
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
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 38;
            Projectile.width = 38;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
        }

        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            switch (ComboAtt)
            {
                case 6:
                    //This npc local hit cooldown time makes it hit multiple times
                    Projectile.localNPCHitCooldown = 3 * ExtraUpdateMult;
                    break;
            }
        }

        protected override float SwingTimeFunction()
        {
            //How long the swing is
            switch (ComboAtt)
            {
                default:
                case 0:
                    return 20;
                case 1:
                    return 20;
                case 2:
                    return 12;
                case 3:
                    return 12;
                case 4:
                    return 18;
                case 5:
                    return 24;
                case 6:
                    return 60;
                    
            }
        }

        protected override void ModifySimpleSwingAI(float targetRotation, float lerpValue, ref float startSwingRot, ref float endSwingRot, ref float swingProgress)
        {
            base.ModifySimpleSwingAI(targetRotation, lerpValue, ref startSwingRot, ref endSwingRot, ref swingProgress);
            switch (ComboAtt)
            {
                case 6:
                    spinCenter = true;
                    spinCenterOffset = 12;
                    float circleRange = MathHelper.TwoPi * 4;
                    startSwingRot = targetRotation - circleRange;
                    endSwingRot = targetRotation + circleRange;
                    swingProgress = lerpValue;
                    break;
            }
        }

        protected override void ModifyOvalSwingAI(float targetRotation, float lerpValue,
            ref float swingXRadius,
            ref float swingYRadius,
            ref float swingRange,
            ref float swingProgress)
        {

            switch (ComboAtt)
            {
                case 0:
                    if(ComboDirection == 1)
                    {
                        OvalRotOffset = 0;       
                    }
                    else
                    {
                        OvalRotOffset = MathHelper.Pi + MathHelper.PiOver2;
                    }
                    swingXRadius = 128;
                    swingYRadius = 64; 
                    swingRange = MathHelper.Pi / 2f;
                    swingProgress = Easing.InOutExpo(lerpValue, 7);
                    break;
                case 1:
                    if (ComboDirection == 1)
                    {
                        OvalRotOffset = 0;
                    }
                    else
                    {
                        OvalRotOffset = MathHelper.Pi + MathHelper.PiOver2;
                    }
                    swingXRadius = 128 ;
                    swingYRadius = 64 ;
                    swingRange = MathHelper.Pi / 2f;
                    swingProgress = Easing.InOutExpo(lerpValue, 7);
                    break;
                case 4:
                    if (ComboDirection == 1)
                    {
                        OvalRotOffset = 0;
                    }
                    else
                    {
                        OvalRotOffset = MathHelper.Pi + MathHelper.PiOver2;
                    }
                    swingXRadius = 128;
                    swingYRadius = 64 ;
                    swingRange = MathHelper.Pi / 2f;
                    swingProgress = Easing.InOutExpo(lerpValue, 7);
                    break;
            }
        }

        protected override void ModifyStabSwingAI(float targetRotation, float lerpValue, ref float stabRange, ref float swingProgress)
        {
            base.ModifyStabSwingAI(targetRotation, lerpValue, ref stabRange, ref swingProgress);
         
            switch (ComboAtt)
            {
                case 2:
                    stabRange = 184;
                    swingProgress = Easing.SpikeOutExpo(lerpValue);
                    break;
                case 3:
                    stabRange = 184;
                    swingProgress = Easing.SpikeOutExpo(lerpValue);
                    break;
                case 5:
                    stabRange = 252;
                    swingProgress = Easing.SpikeOutExpo(lerpValue);
                    break;
            }
        }

        protected override void SwingAI()
        {

            switch (ComboAtt)
            {
                case 0:
                    OvalEasedSwingAI();
                    break;

                case 1:
                    OvalEasedSwingAI();
                    break;

                case 2:
                    StabSwingEasedAI();
                    break;

                case 3:
                    StabSwingEasedAI();
                    break;

                case 4:
                    OvalEasedSwingAI();
                    break;

                case 5:
                    StabSwingEasedAI();
                    break;

                case 6:
                    SimpleEasedSwingAI();
                    break;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (!Hit)
            {
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                Particle.NewParticle<IceStrikeParticle>(target.Center, Vector2.Zero, Color.White);

                Hit = true;
                hitstopTimer = 4 * ExtraUpdateMult;
            }
        }

        //TRAIL VISUALS
        protected override Vector2 GetFramingSize()
        {
            //Set this to the width and height of the sword sprite
            return new Vector2(68, 72);
        }

        protected override Vector2 GetTrailOffset()
        {
            //Moves the trail along the blade, negative goes towards the player, positive goes away the player
            return Vector2.One * 80;
        }

        protected override float WidthFunction(float p)
        {
            float trailWidth = MathHelper.Lerp(0, 252, p);
            float fadeWidth = MathHelper.Lerp(trailWidth, 0, _smoothedLerpValue) * Easing.OutExpo(_smoothedLerpValue, 4);
            return fadeWidth;
        }

        protected override Color ColorFunction(float p)
        {
            Color trailColor = Color.Lerp(Color.White, Color.LightCyan, p);
            Color fadeColor = Color.Lerp(trailColor, Color.DeepSkyBlue, _smoothedLerpValue);
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
            shader.TertiaryTrailingTexture = TextureRegistry.CrystalTrail;
            shader.PrimaryColor = Color.White;
            shader.SecondaryColor = Color.DarkSlateBlue;
            shader.BlendState = BlendState.Additive;
            shader.Speed = 25;
            return shader;
        }
    }
}
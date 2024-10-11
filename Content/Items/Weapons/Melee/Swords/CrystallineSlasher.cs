using CrystalMoon.Content.Bases;
using CrystalMoon.Registries;
using CrystalMoon.Systems;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Players;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Items.Weapons.Melee.Swords
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class CrystallineSlasher : BaseSwingItem
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
            Item.useTime = 126;
            Item.useAnimation = 126;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<CrystallineSwordSlash>();
            Item.autoReuse = true;

            //Combo variables
            //Set combo wait time
            comboWaitTime = 60;

            //Set max combo
            maxCombo = 6;

            //Set stamina to use
            staminaToUse = 1;

            //Set stamina projectile
            staminaProjectileShoot = ModContent.ProjectileType<CrystallineSwordSlash>();
        }
    }

    public class CrystallineSwordSlash : BaseSwingProjectile
    {
        public override string Texture => "CrystalMoon/Content/Items/Weapons/Melee/Swords/CrystallineSlasher";
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
            Projectile.height = 38;
            Projectile.width = 38;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
        }

        public override void SetComboDefaults(List<BaseSwingStyle> swings)
        {
            base.SetComboDefaults(swings);
            swings.Add(new CircleSwingStyle
            {
                swingTime = 24,
                startSwingRotOffset = -MathHelper.ToRadians(135),
                endSwingRotOffset = MathHelper.ToRadians(135),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
            });

            swings.Add(new OvalSwingStyle
            {
                swingTime = 24,
                swingXRadius = 128 / 1.5f,
                swingYRadius = 64 / 1.5f,
                swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4,
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10)
            });

            swings.Add(new OvalSwingStyle
            {
                swingTime = 24,
                swingXRadius = 128 / 1.5f,
                swingYRadius = 64 / 1.5f,
                swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4,
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 24,
                startSwingRotOffset = -MathHelper.ToRadians(135),
                endSwingRotOffset = MathHelper.ToRadians(135),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 7),
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 24,
                startSwingRotOffset = -MathHelper.ToRadians(135),
                endSwingRotOffset = MathHelper.ToRadians(135),
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 7),
            });

            float circleRange = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.TwoPi;
            swings.Add(new CircleSwingStyle
            {
                swingTime = 40,
                startSwingRotOffset = -circleRange,
                endSwingRotOffset = circleRange,
                easingFunc = (float lerpValue) => Easing.InOutExpo(lerpValue, 10),
            });
        }


        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            if(ComboAtt == 5)
            {
                Projectile.localNPCHitCooldown = 2 * ExtraUpdateMult;
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

        
    }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            if (ComboAtt == 5)
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
            return Vector2.One * 80;
        }

        protected override float WidthFunction(float p)
        {
            float trailWidth = MathHelper.Lerp(0, 384, p);
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
        #endregion
    }
}
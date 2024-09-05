using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Players;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using CrystalMoon.Content.Bases;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Items.Weapons.Melee.Swords
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class CrystallineSlasher : ModItem
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
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 60;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);

            comboPlayer.IncreaseCombo(maxCombo: 6);
            return false;
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

        protected override float SwingTimeFunction()
        {
            switch (ComboAtt)
            {
                default:
                case 0:
                    return 24;
                case 1:
                    return 24;
                case 2:
                    return 24;
                case 3:
                    return 24;
                case 4:
                    return 24;
                case 5:
                    return 48;
            }
        }

        protected override void ModifySimpleSwingAI(float targetRotation, float lerpValue,
            ref float startSwingRot,
            ref float endSwingRot,
            ref float swingProgress)
        {
            switch (ComboAtt)
            {
                default:
                case 0:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutExpo(lerpValue, 10);
                    break;
                case 3:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutExpo(lerpValue, 10);
                    break;
                case 4:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutExpo(lerpValue, 10);
                    break;
                case 5:
                    float circleRange = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.TwoPi;
                    startSwingRot = targetRotation - circleRange;
                    endSwingRot = targetRotation + circleRange;
                    swingProgress = Easing.InOutExpo(lerpValue, 10);
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
                case 1:
                    swingXRadius = 128 / 1.5f;
                    swingYRadius = 64 / 1.5f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutExpo(lerpValue, 10);

                    break;
                case 2:
                    swingXRadius = 128 / 1.5f;
                    swingYRadius = 64 / 1.5f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutExpo(lerpValue, 10);
                    break;
            }
        }

        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            switch (ComboAtt)
            {
                case 5:
                    Projectile.localNPCHitCooldown = 2 * ExtraUpdateMult;
                    break;
            }
        }

        protected override void SwingAI()
        {
        
            switch (ComboAtt)
            {
                case 0:
                    SimpleEasedSwingAI();
                    break;

                case 1:
                    OvalEasedSwingAI();
                    break;

                case 2:
                    OvalEasedSwingAI();
                    break;

                case 3:
                    SimpleEasedSwingAI();
                    break;

                case 4:
                    SimpleEasedSwingAI();
                    break;

                case 5:
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
            return Vector2.One * 92;
        }

        protected override float WidthFunction(float p)
        {
            float trailWidth = MathHelper.Lerp(0, 256, p);
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
            shader.TrailingTexture = TrailRegistry.GlowTrail;

            //Blends with the main texture
            shader.SecondaryTrailingTexture = TrailRegistry.GlowTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.TertiaryTrailingTexture = TrailRegistry.CrystalTrail;
            shader.PrimaryColor = Color.White;
            shader.SecondaryColor = Color.DarkSlateBlue;
            shader.BlendState = BlendState.Additive;
            shader.Speed = 25;
            return shader;
        }
    }
}
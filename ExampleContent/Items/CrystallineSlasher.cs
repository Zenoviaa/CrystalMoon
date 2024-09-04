using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Shaders;
using LunarVeil.Content.Bases;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.ExampleContent.Items
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class CrystallineSlasher : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.LunarVeil.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
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
            comboPlayer.ComboWaitTime = 100;

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
        public override string Texture => "CrystalMoon/ExampleContent/Items/CrystallineSlasher";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 32;
        }

        public override void SetDefaults()
        {
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

            //Multiplying by the thing so it's still 10 ticks
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
        }

        protected override void ModifySimpleSwingAI(float targetRotation, float lerpValue, ref float startSwingRot, ref float endSwingRot, ref float swingProgress)
        {  
            switch (ComboAtt)
            {
                case 0:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
                case 3:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
                case 4:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
                case 5:
                    float circleRange = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.TwoPi;
                    startSwingRot = targetRotation - circleRange;
                    endSwingRot = targetRotation + circleRange;
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
            }
        }

        protected override void ModifyOvalSwingAI(float targetRotation, float lerpValue, ref float swingXRadius, ref float swingYRadius, ref float swingRange, ref float swingProgress)
        {
  
            switch (ComboAtt)
            {
                case 1:
                    swingXRadius = 128/1.5f;
                    swingYRadius = 64/1.5f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutExpo(lerpValue, 8);

                    break;
                case 2:
                    swingXRadius = 128/1.5f;
                    swingYRadius = 64/1.5f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutExpo(lerpValue, 8);
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
            if (!Hit)
            {
  //              Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                
//                Particle.NewParticle<IceStrikeParticle>(target.Center,Vector2.Zero, Color.White);

                Hit = true;
                hitstopTimer = 6 * ExtraUpdateMult;
            }
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
            shader.SecondaryColor = Color.LightGoldenrodYellow;
            

            //Main trailing texture
            /*
            var shader = SimpleGradientTrailShader.Instance;
            shader.SlashTexture = TextureRegistry.FXSwordSlash;
            shader.GradientTexture = TextureRegistry.FXSwordSlashGradientBright;
            shader.PrimaryColor = Color.White;
            shader.SecondaryColor = Color.Blue;

            //Alpha Blend/Additive
            shader.BlendState = BlendState.Additive;
            */
            return shader;
        }
    }
}
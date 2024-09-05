using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Systems.Players;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Visual.Particles;
using CrystalMoon.Content.Bases;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace CrystalMoon.Content.Items.Weapons.Melee.Hammer
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class RuneSmasher : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.LunarVeil.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 15;
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
            Item.shoot = ModContent.ProjectileType<RuneHammerSlash>();
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 121;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);

            comboPlayer.IncreaseCombo(maxCombo: 9);
            return false;
        }
    }

    public class RuneHammerSlash : BaseSwingProjectile
    {
        public override string Texture => "CrystalMoon/Content/Items/Weapons/Melee/Hammer/RuneSmasher";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;
        public int BounceTimer;
        public int BounceDelay;
        public bool bounced = false;
        public const int Swing_Speed_Multiplier = 8;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 64;
            ProjectileID.Sets.TrailingMode[Type] = 2;

        }

        public override void SetDefaults()
        {
            holdOffset = 70;
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
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(BounceTimer);
            writer.Write(BounceDelay);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BounceTimer = reader.ReadInt32();
            BounceDelay = reader.ReadInt32();
        }
        protected override float SwingTimeFunction()
        {
            switch (ComboAtt)
            {
                default:
                case 0:
                    return 90;
                case 1:
                    return 90;
                case 2:
                    return 90;
                case 3:
                    return 78;
                case 4:
                    return 78;
                case 5:
                    return 78;
                case 6:
                    return 80;
                case 7:
                    return 100;
                case 8:
                    return 120;
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
                    startSwingRot = targetRotation - MathHelper.ToRadians(155);
                    endSwingRot = targetRotation + MathHelper.ToRadians(155);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
                case 1:
                    startSwingRot = targetRotation - MathHelper.ToRadians(155);
                    endSwingRot = targetRotation + MathHelper.ToRadians(175);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;

                case 2:
                    startSwingRot = targetRotation - MathHelper.ToRadians(175);
                    endSwingRot = targetRotation + MathHelper.ToRadians(225);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;

                case 3:
                    startSwingRot = targetRotation - MathHelper.ToRadians(225);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
                case 4:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
                case 5:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(435);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;

                case 6:
                    startSwingRot = targetRotation - MathHelper.ToRadians(435);
                    endSwingRot = targetRotation + MathHelper.ToRadians(135);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;

                case 7:
                    startSwingRot = targetRotation - MathHelper.ToRadians(135);
                    endSwingRot = targetRotation + MathHelper.ToRadians(435);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;

                case 8:
                    startSwingRot = targetRotation - MathHelper.ToRadians(435);
                    endSwingRot = targetRotation + MathHelper.ToRadians(235);
                    swingProgress = Easing.InOutBack(lerpValue);
                    break;
            }
        }

        protected override void ModifyOvalSwingAI(float targetRotation, float lerpValue,
            ref float swingXRadius,
            ref float swingYRadius,
            ref float swingRange,
            ref float swingProgress)
        {
            /*
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
            */
        }

        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            switch (ComboAtt)
            {
                /*
                case 5:
                    Projectile.localNPCHitCooldown = 2 * ExtraUpdateMult;
                    break;
                */
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
                    SimpleEasedSwingAI();
                    break;

                case 2:
                    SimpleEasedSwingAI();
                   // OvalEasedSwingAI();
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

                case 6:
                    SimpleEasedSwingAI();
                    break;

                case 7:
                    SimpleEasedSwingAI();
                    break;

                case 8:
                    SimpleEasedSwingAI();
                    break;
            }
        }

        private float _hitCount;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 oldMouseWorld = Main.MouseWorld;

            _hitCount++;
            float pitch = MathHelper.Clamp(_hitCount * 0.05f, 0f, 1f);

            /*
            SoundStyle jugglerHit = SoundRegistry.JugglerHit;
            jugglerHit.Pitch = pitch;
            jugglerHit.PitchVariance = 0.1f;
            jugglerHit.Volume = 0.5f;
            SoundEngine.PlaySound(jugglerHit, Projectile.position);

            if (_hitCount >= 7)
            {
                SoundStyle jugglerHitMax = SoundRegistry.JugglerHitMax;
                pitch = MathHelper.Clamp(_hitCount * 0.02f, 0f, 1f);
                jugglerHitMax.Pitch = pitch;
                jugglerHitMax.PitchVariance = 0.1f;
                SoundEngine.PlaySound(jugglerHitMax, Projectile.position);
            }
            

            for (int i = 0; i < 8; i++)
            {
                //Get a random velocity
                Vector2 velocity = Main.rand.NextVector2Circular(4, 4);

                //Get a random
                float randScale = Main.rand.NextFloat(0.5f, 1.5f);
              //  ParticleManager.NewParticle<StarParticle2>(target.Center, velocity, Color.DarkGoldenrod, randScale);
            }
            */
            if (BounceTimer <= 0)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    player.velocity = Projectile.DirectionTo(oldMouseWorld) * -3f;
                }

                BounceTimer = 10 * ExtraUpdateMult;
                BounceDelay = 2 * ExtraUpdateMult;
                Projectile.netUpdate = true;
                
            }
           
            if (BounceDelay > 0)
            {
                BounceDelay--;
            }
            else
            {
                BounceTimer--;
                if (BounceTimer > 0)
                {
                    
                }
            }

            base.OnHitNPC(target, hit, damageDone);
            if (!Hit)
            {
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                //  Particle.NewParticle<IceStrikeParticle>(target.Center, Vector2.Zero, Color.White);
                ComboAtt += 6;
                Hit = true;
                hitstopTimer = 4 * ExtraUpdateMult;
            }
        }

        //TRAIL VISUALS
        protected override Vector2 GetFramingSize()
        {
            //Set this to the width and height of the sword sprite
            return new Vector2(80, 80);
        }

        protected override Vector2 GetTrailOffset()
        {
            //Moves the trail along the blade, negative goes towards the player, positive goes away the player
            return Vector2.One * 92;
        }

        protected override float WidthFunction(float p)
        {
            float trailWidth = MathHelper.Lerp(0, 326, p);
            float fadeWidth = MathHelper.Lerp(trailWidth, 0, _smoothedLerpValue) * Easing.OutExpo(_smoothedLerpValue, 4);
            return fadeWidth;
        }

        protected override Color ColorFunction(float p)
        {
            Color trailColor = Color.Lerp(Color.White, Color.DarkGoldenrod, p);
            Color fadeColor = Color.Lerp(trailColor, Color.DarkGoldenrod, _smoothedLerpValue);
            //This will make it fade out near the end
            return fadeColor;
        }

        protected override BaseShader ReadyShader()
        {

            var shader = SimpleTrailShader.Instance;

            //Main trailing texture
            shader.TrailingTexture = TrailRegistry.LightningTrail;

            //Blends with the main texture
            shader.SecondaryTrailingTexture = TrailRegistry.GlowTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.TertiaryTrailingTexture = TrailRegistry.CrystalTrail;
            shader.PrimaryColor = Color.White;
            shader.SecondaryColor = Color.DarkGoldenrod;
            shader.BlendState = BlendState.AlphaBlend;
            shader.Speed = 25;
            return shader;
        }
    }
}
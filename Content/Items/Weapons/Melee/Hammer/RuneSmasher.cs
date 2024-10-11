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
using System.Collections.Generic;


namespace CrystalMoon.Content.Items.Weapons.Melee.Hammer
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class RuneSmasher : BaseSwingItem
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

            comboWaitTime = 121;
            maxCombo = 9;
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
        public int bounceCount;
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

        public override void SetComboDefaults(List<BaseSwingStyle> swings)
        {
            base.SetComboDefaults(swings);
            swings.Add(new CircleSwingStyle
            {
                swingTime=90,
                startSwingRotOffset = -MathHelper.ToRadians(155),
                endSwingRotOffset = MathHelper.ToRadians(155),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 90,
                startSwingRotOffset = -MathHelper.ToRadians(155),
                endSwingRotOffset = MathHelper.ToRadians(175),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 90,
                startSwingRotOffset = -MathHelper.ToRadians(175),
                endSwingRotOffset = MathHelper.ToRadians(225),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 78,
                startSwingRotOffset = -MathHelper.ToRadians(225),
                endSwingRotOffset = MathHelper.ToRadians(135),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 78,
                startSwingRotOffset = -MathHelper.ToRadians(135),
                endSwingRotOffset = MathHelper.ToRadians(135),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 78,
                startSwingRotOffset = -MathHelper.ToRadians(135),
                endSwingRotOffset = MathHelper.ToRadians(435),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 80,
                startSwingRotOffset = -MathHelper.ToRadians(435),
                endSwingRotOffset = MathHelper.ToRadians(135),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 100,
                startSwingRotOffset = -MathHelper.ToRadians(135),
                endSwingRotOffset = MathHelper.ToRadians(435),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });

            swings.Add(new CircleSwingStyle
            {
                swingTime = 120,
                startSwingRotOffset = -MathHelper.ToRadians(435),
                endSwingRotOffset = MathHelper.ToRadians(235),
                easingFunc = (float lerpValue) => Easing.InOutBack(lerpValue)
            });
        }

        private float _hitCount;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 oldMouseWorld = Main.MouseWorld;

            _hitCount++;
            float pitch = MathHelper.Clamp(_hitCount * 0.05f, 0f, 1f);

            if (bounceTimer <= 0 && bounceCount < 1)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    player.velocity = Projectile.DirectionTo(oldMouseWorld) * -3f;
                }

                bounceTimer = 10 * ExtraUpdateMult;
                Projectile.netUpdate = true;
                
            }


            base.OnHitNPC(target, hit, damageDone);
            if (!Hit)
            {
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                //  Particle.NewParticle<IceStrikeParticle>(target.Center, Vector2.Zero, Color.White);
 
                Hit = true;
                hitstopTimer = 4 * ExtraUpdateMult;
            }
            bounceCount++;
        }


        //TRAIL VISUALS
        #region Trail Visuals
        public override Vector2 GetFramingSize()
        {
            //Set this to the width and height of the sword sprite
            return new Vector2(80, 80);
        }

        public override Vector2 GetTrailOffset()
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
            shader.TrailingTexture = TextureRegistry.LightningTrail;

            //Blends with the main texture
            shader.SecondaryTrailingTexture = TextureRegistry.GlowTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.TertiaryTrailingTexture = TextureRegistry.CrystalTrail;
            shader.PrimaryColor = Color.White;
            shader.SecondaryColor = Color.DarkGoldenrod;
            shader.BlendState = BlendState.AlphaBlend;
            shader.Speed = 25;
            return shader;
        }
        #endregion
    }
}
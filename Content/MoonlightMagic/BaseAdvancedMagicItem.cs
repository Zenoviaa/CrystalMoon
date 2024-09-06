using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal abstract class BaseAdvancedMagicItem : ModItem
    {
        public Texture2D Form { get; set;}
        public BaseMovement Movement { get; set; }
        public int TrailLength { get; set; }
        public UnifiedRandom Random { get; private set; }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 18;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;

            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<AdvancedMagicProjectile>();
            Item.autoReuse = true;
            TrailLength = 32;

            //Randomize trail values
            int seed = WorldGen._genRandSeed;
            Random = new UnifiedRandom(seed);
            TrailLength = Random.Next(24, 32);
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            AdvancedMagicUtil.NewMagicProjectile(this, player, source, position, velocity, type, damage, knockback);
            return false;
        }
    }
}

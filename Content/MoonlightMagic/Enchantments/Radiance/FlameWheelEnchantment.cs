using CrystalMoon.Content.Bases;
using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using CrystalMoon.Systems.MiscellaneousMath;
using CrystalMoon.Systems.ScreenSystems;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Radiance
{
    internal class FlameWheelEnchantment : BaseEnchantment
    {
        private bool _summonedCircle;
        private float _shootDelay;
        private float _shootWaitTime;
        private Vector2 _spawnOffset;
        public override void SetMagicDefaults()
        {
            base.SetMagicDefaults();
            float minWaitTime = 20;
            float maxWaitTime = 80;

            Player player = Main.player[Projectile.owner];
            _spawnOffset = Projectile.Center - player.Center;
            Vector2 directionToProj =_spawnOffset.SafeNormalize(Vector2.Zero);
            float rotToProj = directionToProj.ToRotation();
            float progress = rotToProj / MathHelper.TwoPi;
            _shootWaitTime = MathHelper.Lerp(minWaitTime, maxWaitTime, progress);
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<RadianceElement>();
        }

        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override void AI()
        {
            base.AI();
            if(!_summonedCircle && !MagicProj.IsClone)
            {
                float count = 8;
                float spawnRadius = 48;
                Player player = Main.player[Projectile.owner];
                for(float f = 0; f < count; f++)
                {
                    float progress = f / count;
                    float rot = progress * MathHelper.TwoPi;
                    Vector2 directionToMoveIn = rot.ToRotationVector2();
                    Vector2 spawnOffset = directionToMoveIn * spawnRadius;
                    Vector2 spawnPos = player.Center + spawnOffset;

                    float damage = Projectile.damage / count;
                    int finalDamage = (int)damage;
                    AdvancedMagicUtil.CloneMagicProjectile(MagicProj, spawnPos, Projectile.velocity, finalDamage, Projectile.knockBack / count,
                        MagicProj.TrailLength, MagicProj.Size / count);
                }
        

                Projectile.Kill();
                _summonedCircle=true;
            } 
            //If you are a clone then you already fired lmao
            else if (MagicProj.IsClone)
            {
                _shootDelay++;
                if(_shootDelay < _shootWaitTime)
                {         
                    Player player = Main.player[Projectile.owner];
                    Projectile.Center = player.Center + _spawnOffset;
                }
            }
        }
    }
}

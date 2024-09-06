using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class MoonlightMagicProjectile : ModProjectile
    {
        public override string Texture => TextureRegistry.EmptyTexturePath;

        public Vector2[] OldPos { get; private set; }
        public float Size { get; set; }
        public int TrailLength { get; set; }
        public float GlobalTimer
        {
            get
            {
                return Projectile.ai[0];
            }
            private set
            {
                Projectile.ai[0] = value;
            }
        }

        public BaseForm Form { get; private set; }
        public BaseMovement Movement { get; private set; }
        public BaseElement PrimaryElement { get; private set; }
        public List<BaseEnchantment> Enchantments { get; private set; } = new List<BaseEnchantment>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.timeLeft = 360;
        }

        public void SetMoonlightDefaults(
            BaseElement primaryElement,
            BaseForm form,
            BaseMovement movement,
            List<BaseEnchantment> enchantments)
        {
            PrimaryElement = primaryElement;
            Movement = movement;
            Form = form;
            Enchantments.Clear();
            for (int i = 0; i < enchantments.Count; i++)
            {
                var enchantment = enchantments[i];
                enchantment.MagicProj = this;
                enchantment.SetDefaults();
                Enchantments.Add(enchantment);
            }

            if(PrimaryElement != null)
                PrimaryElement.MagicProj = this;
            if (Movement != null)
                Movement.MagicProj = this;
            if (Form != null)
                Form.MagicProj = this;
            OldPos = new Vector2[TrailLength];
        }

        public override void AI()
        {
            base.AI();
            PrimaryElement?.AI();
            Movement?.AI();
            GlobalTimer++;
            for (int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.AI();
            }

            for(int i = OldPos.Length - 1; i > 0; i--)
            {
                OldPos[i] = OldPos[i - 1];
            }
            OldPos[0] = Projectile.position;

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            for (int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.OnHitNPC(target, hit, damageDone);
            }
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            for (int i = 0; i < Enchantments.Count; i++)
            {
                var enchantment = Enchantments[i];
                enchantment.OnKill(timeLeft);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PrimaryElement?.DrawTrail();
            Form?.PreDraw(ref lightColor);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Form?.PostDraw(lightColor);
            base.PostDraw(lightColor);
        }
    }
}

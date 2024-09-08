using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Registries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic
{
    internal class AdvancedMagicProjectile : ModProjectile
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

        public Texture2D Form { get; private set; }
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

        public void SetMoonlightDefaults(BaseStaff item)
        {
            Projectile.width = Projectile.height = item.Size;
            if (item.primaryElement == null || item.primaryElement.ModItem is not BaseElement || item.primaryElement.IsAir)
                PrimaryElement = new BasicElement();
            else
                PrimaryElement = (item.primaryElement.ModItem as BaseElement).Instantiate();
            Movement = item.Movement;
            Form = item.Form;
            Enchantments.Clear();

            var enchantments = item.equippedEnchantments;
            for (int i = 0; i < enchantments.Length; i++)
            {
                var enchantmentTemplate = enchantments[i];
                if (enchantmentTemplate == null)
                    continue;

                var modItem = enchantments[i].ModItem;
                if(modItem is BaseEnchantment enchantment)
                {
                    var instance = enchantment.Instantiate();
                    instance.MagicProj = this;
                    instance.SetDefaults();
                    Enchantments.Add(instance);
                }
            }

         
            if(PrimaryElement != null)
                PrimaryElement.MagicProj = this;
            if (Movement != null)
                Movement.MagicProj = this;
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
            if(Form != null)
            {
                PrimaryElement?.ApplyFormShader();
                SpriteBatch spriteBatch = Main.spriteBatch;
                Vector2 drawPos = Main.screenPosition - Projectile.Center;
                Vector2 drawOrigin = Form.Size() / 2;
                Color color = Color.White.MultiplyRGB(lightColor);
                spriteBatch.Draw(Form, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                
                //Exit Shader Region
                spriteBatch.End();
                spriteBatch.Begin();
            }

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
        }
    }
}

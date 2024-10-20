using CrystalMoon.Registries;
using CrystalMoon.Systems.Shaders;
using CrystalMoon.Systems.Shaders.MagicTrails;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystalMoon.Content.Items.Dyes
{
    internal class MagicalDyeArmorShaderData : ArmorShaderData
    {
        public MagicalDyeArmorShaderData(Ref<Effect> shader, string passName)
            : base(shader, passName)
        {
        }
        public MagicalDyeArmorShaderData(Asset<Effect> shader, string passName)
            : base(shader, passName)
        {
        }
        public Asset<Texture2D> PrimaryTexture { get; set; }
        public Asset<Texture2D> NoiseTexture { get; set; }
        public Asset<Texture2D> OutlineTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color NoiseColor { get; set; }
        public Color OutlineColor { get; set; }
        public float Speed { get; set; }
        public float Distortion { get; set; }
        public float Power { get; set; }
        public float Blend { get; set; }
        public override void Apply()
        {
            PixelMagicVaellusShader shader = PixelMagicVaellusShader.Instance;
            PrimaryTexture = TextureRegistry.NoiseTextureCloudsSmall;
            NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            OutlineTexture = TextureRegistry.LightningTrail2Outline;
            PrimaryColor = new Color(69, 70, 159);
            NoiseColor = new Color(224, 107, 10);
            OutlineColor = Color.Lerp(new Color(31, 27, 59), Color.Black, 0.75f);
            Speed = 5.2f;
            Distortion = 0.15f;
            Power = 0.25f;
            Blend = 0.4f;

            Shader.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Shader.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Shader.Parameters["noiseColor"].SetValue(NoiseColor.ToVector3());
            Shader.Parameters["outlineColor"].SetValue(OutlineColor.ToVector3());
            Shader.Parameters["primaryTexture"].SetValue(PrimaryTexture.Value);
            Shader.Parameters["noiseTexture"].SetValue(NoiseTexture.Value);
            Shader.Parameters["outlineTexture"].SetValue(OutlineTexture.Value);
            Shader.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
            Shader.Parameters["distortion"].SetValue(Distortion);
            Shader.Parameters["power"].SetValue(Power);
            Shader.Parameters["blend"].SetValue(Blend);
            base.Apply();
        }
    }

    public class MagicalDye : ModItem
    {
        private static MagicalDyeArmorShaderData _armorShader;
        public override void SetStaticDefaults()
        {
            // Avoid loading assets on dedicated servers. They don't use graphics cards.
            if (!Main.dedServ)
            {
                // The following code creates an effect (shader) reference and associates it with this item's type Id.
                _armorShader = GameShaders.Armor.BindShader(
                    Item.type,
                    new MagicalDyeArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/PixelMagicVaellus"), "PixelPass") // Be sure to update the effect path and pass name here.
                );

            }

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            // Item.dye will already be assigned to this item prior to SetDefaults because of the above GameShaders.Armor.BindShader code in Load().
            // This code here remembers Item.dye so that information isn't lost during CloneDefaults.
            int dye = Item.dye;

            Item.CloneDefaults(ItemID.GelDye); // Makes the item copy the attributes of the item "Gel Dye" Change "GelDye" to whatever dye type you want.

            Item.dye = dye;
        }
    }
}
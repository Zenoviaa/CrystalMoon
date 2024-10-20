using CrystalMoon.Registries;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CrystalMoon.Systems.Shaders.Dyes;

namespace CrystalMoon.Content.Items.Dyes
{
    internal class MothlightDyeArmorShaderData : ArmorShaderData
    {
        public MothlightDyeArmorShaderData(Ref<Effect> shader, string passName)
            : base(shader, passName)
        {
        }
        public MothlightDyeArmorShaderData(Asset<Effect> shader, string passName)
            : base(shader, passName)
        {
        }

        public override void Apply()
        {
            DyeMothlightShader shader = DyeMothlightShader.Instance;
            shader.NoiseTexture = TextureRegistry.NoiseTextureClouds3;
            Shader.Parameters["noiseTexture"].SetValue(shader.NoiseTexture.Value);
            Shader.Parameters["noiseTextureSize"].SetValue(shader.NoiseTexture.Value.Size());
            Shader.Parameters["primaryColor"].SetValue(shader.PrimaryColor.ToVector3());
            Shader.Parameters["noiseColor"].SetValue(shader.NoiseColor.ToVector3());
            Shader.Parameters["outlineColor"].SetValue(shader.OutlineColor.ToVector3());
            Shader.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * shader.Speed);
            Shader.Parameters["distortion"].SetValue(shader.Distortion);
            base.Apply();
        }
    }

    public class MothlightDye : ModItem
    {
        private static MothlightDyeArmorShaderData _armorShader;
        public override void SetStaticDefaults()
        {
            // Avoid loading assets on dedicated servers. They don't use graphics cards.
            if (!Main.dedServ)
            {
                // The following code creates an effect (shader) reference and associates it with this item's type Id.
                _armorShader = GameShaders.Armor.BindShader(
                    Item.type,
                    new MothlightDyeArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/PixelMagicHex"), "PixelPass") // Be sure to update the effect path and pass name here.
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

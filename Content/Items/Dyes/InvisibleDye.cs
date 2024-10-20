using CrystalMoon.Registries;
using CrystalMoon.Systems.Shaders;
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
using CrystalMoon.Systems.Shaders.Dyes;

namespace CrystalMoon.Content.Items.Dyes
{
    internal class InvisibleDyeArmorShaderData : ArmorShaderData
    {
        public InvisibleDyeArmorShaderData(Ref<Effect> shader, string passName)
            : base(shader, passName)
        {
        }
        public InvisibleDyeArmorShaderData(Asset<Effect> shader, string passName)
            : base(shader, passName)
        {
        }

        public override void Apply()
        {
            DyeInvisibleShader shader = DyeInvisibleShader.Instance;
            Shader.Parameters["noiseTexture"].SetValue(shader.NoiseTexture.Value);
            Shader.Parameters["noiseTextureSize"].SetValue(shader.NoiseTexture.Value.Size());
            Shader.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * shader.Speed);
            base.Apply();
        }
    }

    public class InvisibleDye : ModItem
    {
        private static InvisibleDyeArmorShaderData _armorShader;
        public override void SetStaticDefaults()
        {
            // Avoid loading assets on dedicated servers. They don't use graphics cards.
            if (!Main.dedServ)
            {
                // The following code creates an effect (shader) reference and associates it with this item's type Id.
                _armorShader = GameShaders.Armor.BindShader(
                    Item.type,
                    new InvisibleDyeArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/DyeInvisible"), "PixelPass") // Be sure to update the effect path and pass name here.
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

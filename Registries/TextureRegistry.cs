using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CrystalMoon.Registries
{
    internal static class TextureRegistry
    {
        public static string EmptyTexturePath => "CrystalMoon/Assets/Textures/Empty";
        public static Asset<Texture2D> NoiseTextureClouds => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Clouds");
        public static Asset<Texture2D> NoiseTextureClouds3 => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Clouds3");
        public static Asset<Texture2D> NoiseTextureWater3 => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Water3");
        public static Asset<Texture2D> SmokeTransparent => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/SmokeTransparent");
        public static Asset<Texture2D> NoiseTextureStars => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Stars");
        public static Asset<Texture2D> FXSwordSlash => ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/FX/SwordSlash");
        public static Asset<Texture2D> FXSwordSlashGradientBright => ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/FX/SwordSlashGradientBright");
        public static Asset<Texture2D> NoiseTextureCloudsSmall => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/SmallClouds");
        public static Asset<Texture2D> NoiseTextureFog => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Fog");
        public static Asset<Texture2D> NoiseTextureFogEmpty => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/FogEmpty");
        public static Asset<Texture2D> NoiseTextureStarsSmall => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/StarsSmall");
        public static Asset<Texture2D> NoiseTextureLeaves => 
            ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Leaves");


        public static string AssetDirectory => "CrystalMoon/Assets/NoiseTextures/";
        public static Asset<Texture2D> BeamTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}BeamTrail");
        public static Asset<Texture2D> BloodletTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}BloodletTrail");
        public static Asset<Texture2D> DottedTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}DottedTrail");
        public static Asset<Texture2D> DottedTrailOutline =>
            ModContent.Request<Texture2D>($"{AssetDirectory}DottedTrailOutline");
        public static Asset<Texture2D> SlashTrail1 =>
            ModContent.Request<Texture2D>($"{AssetDirectory}SlashTrail1");
        public static Asset<Texture2D> GlowTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}GlowTrail");
        public static Asset<Texture2D> SpikyTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}SpikyTrail");
        public static Asset<Texture2D> StarTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}StarTrail");
        public static Asset<Texture2D> WhispTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}SmallWhispyTrail");
        public static Asset<Texture2D> WaterTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}WaterTrail");
        public static Asset<Texture2D> VortexTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}VortexTrail");
        public static Asset<Texture2D> LightningTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}LightningTrail");
        public static Asset<Texture2D> CausticTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}CausticTrail");
        public static Asset<Texture2D> CrystalTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}CrystalTrail");

        public static Asset<Texture2D> CrystalTrail2 =>
           ModContent.Request<Texture2D>($"{AssetDirectory}CrystalTrail2");
        public static Asset<Texture2D> TerraTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}TerraTrail");

    }
}

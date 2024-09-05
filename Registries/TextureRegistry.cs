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
        public static Asset<Texture2D> NoiseTextureStars => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/Stars");
        public static Asset<Texture2D> FXSwordSlash => ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/FX/SwordSlash");
        public static Asset<Texture2D> FXSwordSlashGradientBright => ModContent.Request<Texture2D>("CrystalMoon/Assets/Textures/FX/SwordSlashGradientBright");
        public static Asset<Texture2D> NoiseTextureCloudsSmall => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/SmallClouds");
        public static Asset<Texture2D> NoiseTextureStarsSmall => ModContent.Request<Texture2D>("CrystalMoon/Assets/NoiseTextures/StarsSmall");
    }
}

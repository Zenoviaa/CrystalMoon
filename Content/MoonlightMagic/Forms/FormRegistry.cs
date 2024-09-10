using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CrystalMoon.Content.MoonlightMagic.Forms
{
    internal static class FormRegistry
    {
        public static string AssetDirectory => "CrystalMoon/Content/MoonlightMagic/Forms/";
        public static Asset<Texture2D> Circle => ModContent.Request<Texture2D>(AssetDirectory + "Circle");
        public static Asset<Texture2D> FourPointedStar => ModContent.Request<Texture2D>(AssetDirectory + "FourPointedStar");
    }
}

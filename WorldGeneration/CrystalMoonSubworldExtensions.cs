using Terraria;
using Terraria.ID;

namespace CrystalMoon.WorldGeneration
{
    internal static class CrystalMoonSubworldExtensions
    {
        public static void Active(this Tile tile, bool active)
        {
            tile.HasTile = active;
        }

        public static void SetSlope(this Tile tile, int slope)
        {

            tile.Slope = (SlopeType)slope;
        }
    }
}

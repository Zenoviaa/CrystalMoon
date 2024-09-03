using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;
using static Terraria.WorldGen;

namespace CrystalMoon.Systems.TileSystems
{
    public static class MultitileHelper
    {

        public static Vector2 TileAdj => (Lighting.Mode == Terraria.Graphics.Light.LightMode.Retro || Lighting.Mode == Terraria.Graphics.Light.LightMode.Trippy) ? Vector2.Zero : Vector2.One * 12;

        public static void PlaceMultitile(Point16 position, int type, int style = 0)
        {
            var data = TileObjectData.GetTileData(type, style); //magic numbers and uneccisary params begone!

            if (position.X + data.Width > Main.maxTilesX || position.X < 0)
                return; //make sure we dont spawn outside of the world!

            if (position.Y + data.Height > Main.maxTilesY || position.Y < 0)
                return;

            int xVariants = 0;
            int yVariants = 0;

            if (data.StyleHorizontal)
                xVariants = Main.rand.Next(data.RandomStyleRange);
            else
                yVariants = Main.rand.Next(data.RandomStyleRange);

            for (int x = 0; x < data.Width; x++) //generate each column
            {
                for (int y = 0; y < data.Height; y++) //generate each row
                {
                    Tile tile = Framing.GetTileSafely(position.X + x, position.Y + y); //get the targeted tile
                    tile.TileType = (ushort)type; //set the type of the tile to our multitile

                    int yHeight = 0;
                    for (int k = 0; k < data.CoordinateHeights.Length; k++)
                    {
                        yHeight += data.CoordinateHeights[k] + data.CoordinatePadding;
                    }

                    tile.TileFrameX = (short)((x + data.Width * xVariants) * (data.CoordinateWidth + data.CoordinatePadding)); //set the X frame appropriately
                    tile.TileFrameY = (short)(y * (data.CoordinateHeights[y > 0 ? y - 1 : y] + data.CoordinatePadding) + yVariants * yHeight); //set the Y frame appropriately
                    tile.HasTile = true; //activate the tile
                }
            }
        }

        /// <summary>
        /// returns true if every tile in a rectangle is air
        /// </summary>
        /// <param name="position"></param>might be that f
        /// <param name="size"></param>
        /// <returns></returns>
        
    }
}
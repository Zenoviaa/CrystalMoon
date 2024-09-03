
using CrystalMoon.WorldGeneration;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CrystalMoon.Tiles.AbysmTiles
{
    public class AbyssalDirt : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[TileID.IceBlock][Type] = true;
            Main.tileMerge[TileID.SnowBlock][Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(137, 155, 232));
        }
   
        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            //Tile tileAbove = Framing.GetTileSafely(i, j - 1);

            /*
            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == 0)//grass
            {
                if (Main.rand.NextBool(3))
                {
                    WorldGen.PlaceTile(i, j - 1, TileType<BlueFlower>(), true);
                }
            }
            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == 0)//grass
            {
                if (Main.rand.NextBool(3))
                {
                    WorldGen.PlaceTile(i, j - 1, TileType<BlueFlower2>(), true);
                }
            }
            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == 0)//grass
            {
                if (Main.rand.NextBool(2))
                {
                    WorldGen.PlaceTile(i, j - 2, TileType<TealBulb>(), true);
                }
            }
            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == 0)//grass
            {
                if (Main.rand.NextBool(2))
                {
                    WorldGen.PlaceTile(i, j, TileType<TealBulb2>(), true);
                }
            }
            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == 0)//grass
            {
                if (Main.rand.NextBool(2))
                {
                    WorldGen.PlaceTile(i, j - 1, TileType<TealBulb3>(), true);
                }
            }
            //Try place vine
            */
            if (WorldGen.genRand.NextBool(3) && !tileBelow.HasTile && !(tileBelow.LiquidType == LiquidID.Lava))
            {
                if (!tile.BottomSlope)
                {
                    tileBelow.TileType = (ushort)ModContent.TileType<AbyssalVines>();
                    tileBelow.HasTile = true;
                    WorldGen.SquareTileFrame(i, j + 1, true);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
                    }
                }
            }
            if (WorldGen.genRand.NextBool(3) && !tileBelow.HasTile && !(tileBelow.LiquidType == LiquidID.Lava))
            {
                if (!tile.BottomSlope)
                {
                    tileBelow.TileType = (ushort)ModContent.TileType<AbyssalVines2>();
                    tileBelow.HasTile = true;
                    WorldGen.SquareTileFrame(i, j + 1, true);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
                    }
                }
            }
        }

        private List<Point> OpenAdjacents(int i, int j, int type)
        {
            var p = new List<Point>();
            for (int k = -1; k < 2; ++k)
                for (int l = -1; l < 2; ++l)
                    if (!(l == 0 && k == 0) && Framing.GetTileSafely(i + k, j + l).HasTile && Framing.GetTileSafely(i + k, j + l).TileType == type)
                        p.Add(new Point(i + k, j + l));
            return p;
        }

        private bool HasOpening(int i, int j)
        {
            for (int k = -1; k < 2; ++k)
                for (int l = -1; l < 2; ++l)
                    if (!Framing.GetTileSafely(i + k, j + l).HasTile)
                        return true;
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
        }
    }

        [TileTag(TileTags.VineSway)]
        public class AbyssalVines : ModTile
        {
            public override void SetStaticDefaults()
            {
                Main.tileCut[Type] = true;
                Main.tileLavaDeath[Type] = true;
                Main.tileNoFail[Type] = true;
                Main.tileNoAttach[Type] = true;
                Main.tileLighted[Type] = true;


                TileID.Sets.VineThreads[Type] = true;
                TileID.Sets.IsVine[Type] = true;

                HitSound = SoundID.Grass;
                DustType = DustID.Plantera_Green;

                AddMapEntry(new Color(93, 203, 243));
            }

            public override void NumDust(int i, int j, bool fail, ref int num) => num = 4;

            public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
            {
                Tile tile = Framing.GetTileSafely(i, j + 1);
                if (tile.HasTile && tile.TileType == Type)
                {
                    WorldGen.KillTile(i, j + 1);
                }
            }
            public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
            {
                r = .154f * 2;
                g = .077f * 2;
                b = .255f * 2;
            }
            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                Tile tileAbove = Framing.GetTileSafely(i, j - 1);
                int type = -1;
                if (tileAbove.HasTile && !tileAbove.BottomSlope)
                {
                    type = tileAbove.TileType;
                }

                if (type == ModContent.TileType<AbyssalDirt>() || type == Type)
                {
                    return true;
                }

                WorldGen.KillTile(i, j);
                return true;
            }

            public override void RandomUpdate(int i, int j)
            {
                Tile tileBelow = Framing.GetTileSafely(i, j + 1);
                if (WorldGen.genRand.NextBool(2) && !tileBelow.HasTile && !(tileBelow.LiquidType == LiquidID.Lava))
                {
                    bool placeVine = false;
                    int yTest = j;
                    while (yTest > j - 10)
                    {
                        Tile testTile = Framing.GetTileSafely(i, yTest);
                        if (testTile.BottomSlope)
                        {
                            break;
                        }
                        else if (!testTile.HasTile || testTile.TileType != ModContent.TileType<AbyssalDirt>())
                        {
                            yTest--;
                            continue;
                        }
                        placeVine = true;
                        break;
                    }
                    if (placeVine)
                    {
                        tileBelow.TileType = Type;
                        tileBelow.HasTile = true;
                        WorldGen.SquareTileFrame(i, j + 1, true);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
                        }
                    }
                }
            }



            public float GetOffset(int i, int j, int frameX, float sOffset = 0f)
            {
                float sin = (float)Math.Sin((Main.time + (i * 24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1)) + sOffset) * 1.4f;
                if (Framing.GetTileSafely(i, j - 1).TileType != Type) //Adjusts the sine wave offset to make it look nicer when closer to ground
                    sin *= 0.25f;
                else if (Framing.GetTileSafely(i, j - 2).TileType != Type)
                    sin *= 0.5f;
                else if (Framing.GetTileSafely(i, j - 3).TileType != Type)
                    sin *= 0.75f;

                return sin;
            }
        }

    public class AbyssalVines2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;


            TileID.Sets.VineThreads[Type] = true;
            TileID.Sets.IsVine[Type] = true;

            HitSound = SoundID.Grass;
            DustType = DustID.Plantera_Green;

            AddMapEntry(new Color(93, 243, 243));
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 4;

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Framing.GetTileSafely(i, j + 1);
            if (tile.HasTile && tile.TileType == Type)
            {
                WorldGen.KillTile(i, j + 1);
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .154f * 2;
            g = .177f * 2;
            b = .255f * 2;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileAbove = Framing.GetTileSafely(i, j - 1);
            int type = -1;
            if (tileAbove.HasTile && !tileAbove.BottomSlope)
            {
                type = tileAbove.TileType;
            }

            if (type == ModContent.TileType<AbyssalDirt>() || type == Type)
            {
                return true;
            }

            WorldGen.KillTile(i, j);
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            if (WorldGen.genRand.NextBool(2) && !tileBelow.HasTile && !(tileBelow.LiquidType == LiquidID.Lava))
            {
                bool placeVine = false;
                int yTest = j;
                while (yTest > j - 10)
                {
                    Tile testTile = Framing.GetTileSafely(i, yTest);
                    if (testTile.BottomSlope)
                    {
                        break;
                    }
                    else if (!testTile.HasTile || testTile.TileType != ModContent.TileType<AbyssalDirt>())
                    {
                        yTest--;
                        continue;
                    }
                    placeVine = true;
                    break;
                }
                if (placeVine)
                {
                    tileBelow.TileType = Type;
                    tileBelow.HasTile = true;
                    WorldGen.SquareTileFrame(i, j + 1, true);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
                    }
                }
            }
        }



        public float GetOffset(int i, int j, int frameX, float sOffset = 0f)
        {
            float sin = (float)Math.Sin((Main.time + (i * 24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1)) + sOffset) * 1.4f;
            if (Framing.GetTileSafely(i, j - 1).TileType != Type) //Adjusts the sine wave offset to make it look nicer when closer to ground
                sin *= 0.25f;
            else if (Framing.GetTileSafely(i, j - 2).TileType != Type)
                sin *= 0.5f;
            else if (Framing.GetTileSafely(i, j - 3).TileType != Type)
                sin *= 0.75f;

            return sin;
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CrystalMoon.WorldGeneration.BaseEdits
{
    public class CrystalMoonWorldGenTest : ModSystem
    {
        public static bool JustPressed(Keys key)
        {
            return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
        }

        public override void PostUpdateWorld()
        {
            if (JustPressed(Keys.D1))
                TestMethod((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        }

        private void WormCaves()
        {
                    var genRand = WorldGen.genRand;
            for (int i = 0; i < 16; i++)
            {
                int caveWidth = 12; // Width
                int caveSteps = 250; // How many carves
                int caveSeed = WorldGen.genRand.Next();
                Vector2 baseCaveDirection = Vector2.UnitY.RotatedBy(WorldGen.genRand.NextFloatDirection() * 0.54f);
                Vector2 cavePosition = new Vector2(Main.maxTilesX / 2, (int)Main.worldSurface);

                for (int j = 0; j < caveSteps; j++)
                {
                    float caveOffsetAngleAtStep = WorldMath.PerlinNoise2D(i / 50f, j / 50f, 1, caveSeed) * MathHelper.Pi;
                    Vector2 caveDirection = baseCaveDirection.RotatedBy(caveOffsetAngleAtStep);

                    // Carve out at the current position.
                    if (cavePosition.X < Main.maxTilesX - 15 && cavePosition.X >= 15)
                    {
                        //digging 
                        WorldGen.TileRunner(
                            (int)cavePosition.X,
                            (int)cavePosition.Y,
                            strength: genRand.NextFloat(10f, 20f),
                            genRand.Next(5, 10),
                            type: -1);
                    }

                    // Update the cave position.
                    cavePosition += caveDirection * caveWidth * 0.5f;
                }
            }

        }

        private void NoodleCaves()
        {
            var genRand = WorldGen.genRand;
            for (int i = 0; i < 6; i++)
            {
                int caveWidth = 12; // Width
                int caveSteps = 250; // How many carves
                int caveSeed = WorldGen.genRand.Next();
                Vector2 baseCaveDirection = Vector2.UnitY.RotatedBy(WorldGen.genRand.NextFloatDirection() * 0.54f);
                Vector2 cavePosition = new Vector2(Main.maxTilesX / 2, (int)Main.worldSurface);

                for (int j = 0; j < caveSteps; j++)
                {
                    float caveOffsetAngleAtStep = WorldMath.PerlinNoise2D(i / 50f, j / 50f, 1, caveSeed) * MathHelper.TwoPi * 1.9f;
                    Vector2 caveDirection = baseCaveDirection.RotatedBy(caveOffsetAngleAtStep);

                    // Carve out at the current position.
                    if (cavePosition.X < Main.maxTilesX - 15 && cavePosition.X >= 15)
                    {
                        //digging 
                        WorldGen.TileRunner(
                            (int)cavePosition.X,
                            (int)cavePosition.Y,
                            strength: genRand.NextFloat(10f, 20f),
                            genRand.Next(5, 10),
                            type: -1);
                    }

                    // Update the cave position.
                    cavePosition += caveDirection * caveWidth * 0.5f;
                }
            }
        }
 
        private void LinearCave()
        {
          var genRand = WorldGen.genRand;
            for (int i = 0; i < 6; i++)
            {
                int caveWidth = 12; // Width
                int caveSteps = 250; // How many carves
                int caveSeed = WorldGen.genRand.Next();
                Vector2 baseCaveDirection = Vector2.UnitY.RotatedBy(WorldGen.genRand.NextFloatDirection() * 0.54f);
                Vector2 cavePosition = new Vector2(Main.maxTilesX / 2, (int)Main.worldSurface);

                for (int j = 0; j < caveSteps; j++)
                {
                    float caveOffsetAngleAtStep = WorldMath.PerlinNoise2D(i / 50f, j / caveSteps, 4, caveSeed) * MathHelper.TwoPi * 1.9f;
                    Vector2 caveDirection = baseCaveDirection.RotatedBy(caveOffsetAngleAtStep);

                    // Carve out at the current position.
                    if (cavePosition.X < Main.maxTilesX - 15 && cavePosition.X >= 15)
                    {
                        //digging 
                        WorldGen.TileRunner(
                            (int)cavePosition.X, 
                            (int)cavePosition.Y, 
                            genRand.NextFloat(10f, 20f), 
                            genRand.Next(5, 10), 
                            type: -1);
                    }

                    // Update the cave position.
                    cavePosition += caveDirection * caveWidth * 0.5f;
                }
            }
        }

        private void VeinyCaves()
        {

            var genRand = WorldGen.genRand;
            for (int i = 0; i < 6; i++)
            {
                int caveWidth = 12; // Width
                int caveSteps = 250; // How many carves

                int caveSeed = WorldGen.genRand.Next();
                Vector2 baseCaveDirection = Vector2.UnitY.RotatedBy(WorldGen.genRand.NextFloatDirection() * 0.54f);
                Vector2 cavePosition = new Vector2(Main.maxTilesX / 2, (int)Main.worldSurface);

                for (int j = 0; j < caveSteps; j++)
                {
                    float caveOffsetAngleAtStep = WorldMath.PerlinNoise2D(1 / 50f, j / 50f, 4, caveSeed) * MathHelper.Pi * 1.9f;
                    Vector2 caveDirection = baseCaveDirection.RotatedBy(caveOffsetAngleAtStep);

                    // Carve out at the current position.
                    if (cavePosition.X < Main.maxTilesX - 15 && cavePosition.X >= 15)
                    {
                        //digging 
                        WorldGen.TileRunner((int)cavePosition.X, (int)cavePosition.Y, MathF.Sin(j * 0.05f) * 10 + genRand.NextFloat(2, 5), genRand.Next(5, 10), -1);
                    }

                    // Update the cave position.
                    cavePosition += caveDirection * caveWidth * 0.5f;
                }
            }
        }

        private void TestMethod(int x, int y)
        {

            Dust.QuickBox(new Vector2(x, y) * 16, new Vector2(x + 1, y + 1) * 16, 2, Color.YellowGreen, null);


            /*
            // Code to test placed here:
            for (int i = 0; i < 6; i++)
            {
                int caveWidth = 12; // Width
                int caveSteps = 250; // How many carves

                int caveSeed = WorldGen.genRand.Next();
                Vector2 baseCaveDirection = Vector2.UnitY.RotatedBy(WorldGen.genRand.NextFloatDirection() * 0.54f);
                Vector2 cavePosition = new Vector2(Main.maxTilesX / 2, (int)Main.worldSurface);

                for (int j = 0; j < caveSteps; j++)
                {
                    float caveOffsetAngleAtStep = WorldMath.PerlinNoise2D(1 / 50f, j / 50f, 4, caveSeed) * MathHelper.Pi * 1.9f;
                    Vector2 caveDirection = baseCaveDirection.RotatedBy(caveOffsetAngleAtStep);

                    // Carve out at the current position.
                    if (cavePosition.X < Main.maxTilesX - 15 && cavePosition.X >= 15)
                    {
                        //digging 

                        WorldGen.digTunnel(cavePosition.X, cavePosition.Y, caveDirection.X, caveDirection.Y, 1, (int)(caveWidth * 1.18f), false);
                        WorldUtils.Gen(cavePosition.ToPoint(), new Shapes.Circle(caveWidth, caveWidth), Actions.Chain(new GenAction[]
                        {
                            new Actions.ClearTile(true),
                            new Actions.Smooth(true)
                        }));
                    }

                    // Update the cave position.
                    cavePosition += caveDirection * caveWidth * 0.5f;
                }
            }*/
            // WorldGen.TileRunner(x - 1, y, WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(2, 8), TileID.CobaltBrick);
        }
    }
}

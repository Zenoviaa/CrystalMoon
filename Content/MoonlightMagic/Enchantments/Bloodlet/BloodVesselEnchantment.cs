using CrystalMoon.Content.MoonlightMagic.Elements;
using CrystalMoon.Systems.ScreenSystems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CrystalMoon.Registries;
using CrystalMoon.Content.MoonlightMagic.Movements;
using CrystalMoon.Systems.Particles;
using CrystalMoon.Visual.Particles;
using System.Threading;
using System.Net;
using Terraria.ID;

namespace CrystalMoon.Content.MoonlightMagic.Enchantments.Bloodlet
{
    internal class BloodVesselEnchantment : BaseEnchantment
    {
        private int _timer;

        public Vector2 FindBottomTile(Vector2 position)
        {
            int i = (int)(position.X / 16f);
            int upSide = (int)(position.Y / 16f);
            for(int j = upSide; j < Main.maxTilesY; j++)
            {
                Tile tile = Main.tile[i, j];
                if (WorldGen.SolidTile(i, j))
                {
                    return new Vector2(i * 16, j * 16);
                }
            }

            return position;
        }

        public static Vector4 CustomTileCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fall = false)
        {
            bool[] side = new bool[4];
            int leftSide = (int)(Position.X / 16f) - 1;
            int rightSide = (int)((Position.X + Width) / 16f) + 2;
            int upSide = (int)(Position.Y / 16f) - 1;
            int downSide = (int)((Position.Y + Height) / 16f) + 2;

            // Make sure it is in the world
            leftSide = Utils.Clamp(leftSide, 0, Main.maxTilesX - 1);
            rightSide = Utils.Clamp(rightSide, 0, Main.maxTilesX - 1);
            upSide = Utils.Clamp(upSide, 0, Main.maxTilesY - 1);
            downSide = Utils.Clamp(downSide, 0, Main.maxTilesY - 1);

            Vector2 worldCoord = default;

            for (int i = leftSide; i < rightSide; i++)
            {
                for (int j = upSide; j < downSide; j++)
                {
                    // Ensure tile exist and is solid
                    if (Main.tile[i, j] == null || !Main.tile[i, j].Get<TileWallWireStateData>().HasTile || (!Main.tileSolid[Main.tile[i, j].TileType] && (!Main.tileSolidTop[Main.tile[i, j].TileType] || Main.tile[i, j].TileFrameY != 0)))
                        continue;

                    // Must be a solid tile
                    int slope = (int)Main.tile[i, j].Slope;
                    if (slope != (int)SlopeType.Solid)
                        continue;

                    // Grab and set values and account for half blocks
                    worldCoord.X = i * 16;
                    worldCoord.Y = j * 16;
                    int tileSize = 16;
                    if (Main.tile[i, j].IsHalfBlock)
                    {
                        worldCoord.Y += 8f;
                        tileSize -= 8;
                    }

                    // Size factors
                    if (!(Position.X + Width > worldCoord.X) || !(Position.X < worldCoord.X + 16f) || !(Position.Y + Height > worldCoord.Y) || !(Position.Y < worldCoord.Y + tileSize))
                        continue;

                    float topWorldY = worldCoord.Y - Height;
                    float bottomWorldY = worldCoord.Y - Height + 16f;
                    float leftWorldX = worldCoord.X - Width;
                    float rightWorldX = worldCoord.X - Width + 16f;

                    side[0] = Position.X < rightWorldX;
                    side[1] = Position.X > leftWorldX;
                    side[2] = Position.Y < bottomWorldY;
                    side[3] = Position.Y > topWorldY;

                    // how to get these to run independently of each other?

                    // Right X of the tile
                    if (side[0]) //&& !side[1] && !side[2] && !side[3])
                    {
                        Position.X = rightWorldX;
                        if (Velocity.X > 0f)
                            Velocity.X = 0f;
                    }


                    // Left X of the tile
                    if (side[1]) //&& !side[2] && !side[3] && !side[0])
                    {
                        Position.X = leftWorldX;
                        if (Velocity.X > 0f)
                            Velocity.X = 0f;
                    }

                    // Bottom Y of the tile
                    if (side[2]) //&& !side[3] && !side[0] && !side[1])
                    {
                        Position.Y = bottomWorldY;
                        if (Velocity.Y > 0f)
                            Velocity.Y = 0f;
                    }

                    // Top Y of the tile
                    if (side[3]) //&& !side[0] && !side[1] && !side[2])
                    {
                        Position.Y = topWorldY;
                        if (Velocity.Y > 0f)
                            Velocity.Y = 0f;
                    }

                }
            }

            return new Vector4(Position, Velocity.X, Velocity.Y);
        }
        public override void AI()
        {
            base.AI();

            //Count up
            _timer++;
            if (_timer == 1 && Main.myPlayer == Projectile.owner)
            {
                MagicProj.OldPos[0] = Vector2.Zero;
                Player player = Main.player[Projectile.owner];
                Vector2 center = Main.MouseWorld;
                Vector2 velocity = Vector2.UnitY * 512;

                Vector2 col = FindBottomTile(center);
                Projectile.Center = col - Vector2.UnitY * (MagicProj.Size + 2);
                Projectile.velocity = -Vector2.UnitY * Projectile.velocity.Length();
                Projectile.netUpdate = true;
            }
        }

        public override float GetStaffManaModifier()
        {
            return 0.2f;
        }

        public override int GetElementType()
        {
            return ModContent.ItemType<BloodletElement>();
        }

        public override void SpecialInventoryDraw(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.SpecialInventoryDraw(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            DrawHelper.DrawGlowInInventory(item, spriteBatch, position, ColorUtil.BloodletRed);
        }
    }
}

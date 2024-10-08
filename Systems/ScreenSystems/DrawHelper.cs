﻿using CrystalMoon.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.ScreenSystems
{
    public static class DrawHelper
    {
        public static void RestartSpriteBatch(this SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public static void RestartSpriteBatchInUI(this SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, default, Main.UIScaleMatrix);
        }

        /// <summary>
        /// Oscillates between two colors
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static Color Oscillate(Color from, Color to, float speed)
        {
            float t = VectorHelper.Osc(0, 1, speed);
            return Color.Lerp(from, to, t);
        }


        /// <summary>
        /// Oscillates between two colors, but Huntrian
        /// <br>See Firefly Staff for example usage</br>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="speed"></param>
        /// <param name="colorOffset"></param>
        /// <returns></returns>
        public static Vector3 HuntrianColorOscillate(Vector3 from, Vector3 to, Vector3 speed, float colorOffset)
        {
            Vector3 xyz;
            xyz.X = VectorHelper.Osc(from.X, to.X, speed.X, colorOffset);
            xyz.Y = VectorHelper.Osc(from.Y, to.Y, speed.Y, colorOffset);
            xyz.Z = VectorHelper.Osc(from.Z, to.Z, speed.Z, colorOffset);
            return xyz;
        }


        

        public static void DrawLineTelegraph(Vector2 drawPos, Color drawColor, Vector2 velocity, float drawScale = 1f, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D lineTexture = ModContent.Request<Texture2D>("Stellamod/Effects/Masks/Extra_47").Value;
            Vector2 drawOrigin = lineTexture.Size() / 2;
            float rotation = velocity.ToRotation() + MathHelper.PiOver2;
            spriteBatch.Draw(lineTexture, drawPos, null, drawColor, rotation, drawOrigin, drawScale, spriteEffects, 0);
        }


        public static void DrawLineTelegraph(Vector2 drawPos, Color drawColor, float rotation, float drawScale = 1f, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D lineTexture = ModContent.Request<Texture2D>("Stellamod/Effects/Masks/Extra_47").Value;
            Vector2 drawOrigin = lineTexture.Size() / 2;
            spriteBatch.Draw(lineTexture, drawPos, null, drawColor, rotation, drawOrigin, drawScale, spriteEffects, 0);
        }


        /// <summary>
        /// Draws a simple trail using "VampKnives:BasicTrail"
        /// <br></br>Don't forget to set the trailing cache and trailing modes on your projectile!
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="widthFunction"></param>
        /// <param name="colorFunction"></param>
        /// <param name="trailTexture"></param>
        
        /// <summary>
        /// Draws an after image for the projectile, this should be called in PreDraw
        /// <br>Don't forget to set defaults for ProjectileID.Sets.TrailCacheLength and ProjectileID.Sets.TrailingMode on your projectile otherwise this will not work</br>
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <param name="lightColor"></param>
        public static void DrawAdditiveAfterImage(Projectile projectile, Color startColor, Color endColor, ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            int projFrames = Main.projFrames[projectile.type];
            int frameHeight = texture.Height / projFrames;
            int startY = frameHeight * projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 drawOrigin = sourceRectangle.Size() / 2f;
            //drawOrigin.X = projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;// + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.Lerp(startColor, endColor, 1f / projectile.oldPos.Length * k) * (1f - 1f / projectile.oldPos.Length * k));
                Main.spriteBatch.Draw(texture, drawPos, sourceRectangle, color, projectile.oldRot[k], drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void DrawGlowInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Color glowColor)
        {
            float sizeLimit = 34;
            int numberOfCloneImages = 6;
            Main.DrawItemIcon(spriteBatch, item, position, Color.White * 0.7f, sizeLimit);
            for (float i = 0; i < 1; i += 1f / numberOfCloneImages)
            {
                float cloneImageDistance = MathF.Cos(Main.GlobalTimeWrappedHourly / 2.4f * MathF.Tau / 2f) + 0.5f;
                cloneImageDistance = MathHelper.Max(cloneImageDistance, 0.1f);
                Color color = glowColor * 0.4f;
                color *= 1f - cloneImageDistance * 0.2f;
                color.A = 0;
                cloneImageDistance *= 3;
                Vector2 drawPos = position + (i * MathF.Tau).ToRotationVector2() * (cloneImageDistance + 2f);
                Main.DrawItemIcon(spriteBatch, item, drawPos, color, sizeLimit);
            }
        }

        public static void DrawAdvancedShadingGlow(Item item, SpriteBatch spriteBatch, Vector2 position, Color glowColor)
        {
            float sizeLimit = 34;
            int numberOfCloneImages = 3;
            Main.DrawItemIcon(spriteBatch, item, position, Color.White * 0.2f, sizeLimit);
            for (float i = 0; i < 1; i += 1f / numberOfCloneImages)
            {
                float cloneImageDistance = MathF.Cos(Main.GlobalTimeWrappedHourly / 2.4f * MathF.Tau / 2f) + 0.5f;
                cloneImageDistance = MathHelper.Max(cloneImageDistance, 0.05f);
                Color color = glowColor * 0.4f;
                color *= 1f - cloneImageDistance * 0.2f;
                color.A = 0;
                cloneImageDistance *= 3;
                Vector2 drawPos = position + (i * MathF.Tau).ToRotationVector2() * (cloneImageDistance + 2f);
                Main.DrawItemIcon(spriteBatch, item, drawPos, color, sizeLimit);
            }
        }

        public static void DrawGlow2InWorld(Item item, SpriteBatch spriteBatch, ref float rotation, ref float scale, int whoAmI)
        {
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[item.type].Value;
            Rectangle frame;
            if (Main.itemAnimations[item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(item.width / 2 - frameOrigin.X, item.height - frame.Height);
            Vector2 drawPos = item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;
            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Draws a dim light effect, call this in any draw function
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="dimLightX"></param>
        /// <param name="dimLightY"></param>
        /// <param name="dimLightZ"></param>
      

        /// <summary>
        /// Animates the projectile from top to bottom
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="frameSpeed"></param>
        public static void AnimateTopToBottom(Projectile projectile, int frameSpeed)
        {
            // This is a simple "loop through all frames from top to bottom" animation
            projectile.frameCounter++;
            if (projectile.frameCounter >= frameSpeed)
            {
                projectile.frameCounter = 0;
                projectile.frame++;

                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}

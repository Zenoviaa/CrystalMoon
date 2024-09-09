using CrystalMoon.Registries;
using CrystalMoon.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Systems
{
    internal class FogSystem : ModSystem
    {
        private readonly Dictionary<Point, Fog> _fogIndex = new();
        private readonly List<Fog> _fogsToRemove = new();
        
        public override void Load()
        {
            base.Load();
            On_Main.DrawDust += DrawFog;
        }

        public override void Unload()
        {
            base.Unload();
            On_Main.DrawDust -= DrawFog;
        }

        public Fog SetupFog(Point position, Action<Fog> createFogFunc)
        {
            if (_fogIndex.ContainsKey(position))
                return _fogIndex[position];
            else
            {
                Fog fog = new Fog();
                fog.tilePosition = position;
                fog.position = new Vector2(position.X * 16, position.Y * 16);
                createFogFunc?.Invoke(fog);
                _fogIndex.Add(position, fog);
                return fog;
            }
        }

        private void UpdateFog()
        {
            foreach(var kvp in _fogIndex)
            {
                Fog fog = kvp.Value;
                fog.Update();
                float dist = Vector2.Distance(fog.position, Main.LocalPlayer.position);
                if(dist > 1000)
                {
                    _fogsToRemove.Add(fog);
                }
            }

            for(int i = 0; i < _fogsToRemove.Count; i++)
            {
                Fog fog = _fogsToRemove[i];
                _fogIndex.Remove(fog.tilePosition);
            }
            _fogsToRemove.Clear();
        }

        public override void PostUpdateWorld()
        {
            base.PostUpdateWorld();
            UpdateFog();
          
        }

        private void DrawFog(On_Main.orig_DrawDust orig, Main self)
        {
            orig(self);
            var texture = TextureRegistry.NoiseTextureFogEmpty;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 origin = texture.Size() / 2;

            //Apply Fog Shader
            var fogShader = FogShader.Instance;
            fogShader.FogTexture = TextureRegistry.NoiseTextureFog;
            fogShader.ProgressPower = 1f;
            fogShader.EdgePower = 1f;
            fogShader.Speed = 0.025f;
            fogShader.Apply();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, fogShader.Effect, Main.GameViewMatrix.TransformationMatrix);

            foreach(var kvp in _fogIndex)
            {
                var fog = kvp.Value;
                Vector2 center = fog.position - Main.screenPosition;
                spriteBatch.Draw(texture.Value, center, null, fog.color, 0, origin, fog.scale, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
        }
    }
}

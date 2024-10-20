using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.Shaders
{
    internal interface IDrawPrims
    {
        void DrawPrims(ref PrimitiveDraw draw);
    }

    internal class PrimitiveDraw
    {
        public Vector2[] OldPos;
        public float[] OldRot;
        public Func<float, Color> ColorFunction;
        public Func<float, float> WidthFunction;
        public Vector2? Offset = null;
    }

    internal interface IPrimitivesBatch
    {
        public List<IDrawPrims> Prims { get; }
        public BaseShader Shader { get; }
        public BlendState BlendState { get;}
        public SamplerState SamplerState { get; }
    }

    internal class PrimitivesBatchDrawSystem : ModSystem
    {
        private List<IPrimitivesBatch> _primBatches; 
        public override void Load()
        {
            base.Load();
            _primBatches = new List<IPrimitivesBatch>();
            foreach (Type type in CrystalMoon.Instance.Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetInterfaces().Contains(typeof(IPrimitivesBatch)))
                {
                    object instance = Activator.CreateInstance(type);
                    _primBatches.Add(instance as IPrimitivesBatch);
                }
            }

            On_Main.DoDraw_DrawNPCsOverTiles += DrawPrimitiveBatches;
        }

        public override void Unload()
        {
            base.Unload();
            On_Main.DoDraw_DrawNPCsOverTiles -= DrawPrimitiveBatches;
        }

        private void DrawPrimitiveBatches(On_Main.orig_DoDraw_DrawNPCsOverTiles orig, Main self)
        {
            orig(self);

            //This should be in front of NPCS
            //Just need to loop over all of them and draw them
            //We do it this way so it's super duper optimized
            //Uhhh
            for (int i = 0; i < _primBatches.Count; i++)
            {
                IPrimitivesBatch primitivesBatch = _primBatches[i];
                TrailDrawer.DrawPrimitivesBatch(primitivesBatch);
            }
       
        }
    }
}

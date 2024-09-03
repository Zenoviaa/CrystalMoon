using CrystalMoon.Systems.LoadingSystems;
using CrystalMoon.Systems.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CrystalMoon.Systems
{
    internal static class CrystalMoonUtils
    {

        public static AssetRepository Assets => CrystalMoon.Instance.Assets;

        private static List<IOrderedLoadable> _loadCache;
        public static MiscShaderData CloudsShader => GameShaders.Misc["LunarVeil:Clouds"];
        public static MiscShaderData CloudsFrontShader => GameShaders.Misc["LunarVeil:CloudsFront"];
        public static MiscShaderData NightCloudsShader => GameShaders.Misc["LunarVeil:NightClouds"];
        public static MiscShaderData CloudsDesertShader => GameShaders.Misc["LunarVeil:CloudsDesert"];
        public static MiscShaderData CloudsDesertNightShader => GameShaders.Misc["LunarVeil:CloudsDesertNight"];
        public static Filter WaterFilter => Filters.Scene["LunarVeil:Water"];
        public static Filter WaterBasicFilter => Filters.Scene["LunarVeil:WaterBasic"];
        public static Filter LavaFilter => Filters.Scene["LunarVeil:Lava"];
        public static MiscShaderData GradientShader => GameShaders.Misc["LunarVeil:Gradient"];
        public static Filter CloudySkyFilter => Filters.Scene["LunarVeil:CloudySky"];
        public static Filter DesertSkyFilter => Filters.Scene["LunarVeil:DesertSky"];
        

        public static string Screen_Black => "LunarVeil:Black";
        public static string Screen_Tint => "LunarVeil:Tint";
        public static string Screen_NormalDistortion => "LunarVeil:NormalDistortion";
        public static string Screen_Vignette => "LunarVeil:Vignette";

        public static string VampKnives_Basic_Trail => "VampKnives:BasicTrail";
        public static string VampKnives_Lightning_Trail => "VampKnives:LightningTrail";
        public static string VampKnives_Generic_Laser_Shader => "VampKnives:GenericLaserShader";
        public static string VampKnives_Light_Beam_Vertex_Shader => "VampKnives:LightBeamVertexShader";
        public static string VampKnives_Artemis_Laser_Shader => "VampKnives:ArtemisLaserShader";
        public static string VampKnives_Fire => "VampKnives:Fire";
        public static string LunarVeilFireWhiteShader => "VampKnives:FireWhite";


        private static string GlowingDustShader => "LunarVeil:GlowingDust";
        public static MiscShaderData MiscGlowingDust => GameShaders.Misc[GlowingDustShader];



        private static string Silhouette_Shader => "LunarVeil:SilhouetteShader";
        private static string FireWhitePixelShaderName => "LunarVeil:FireWhitePixelShader";
        public static MiscShaderData MiscFireWhitePixelShader => GameShaders.Misc[FireWhitePixelShaderName];

        private static string TestPixelShaderName => "LunarVeil:TestPixelShader";
        public static MiscShaderData MiscTestPixelShader => GameShaders.Misc[TestPixelShaderName];

        private static string SilShaderName => "LunarVeil:SilShader";
        public static MiscShaderData MiscSilPixelShader => GameShaders.Misc[SilShaderName];

        private static string DistortionShaderName => "LunarVeil:DistortionShader";
        public static MiscShaderData MiscDistortionShader => GameShaders.Misc[DistortionShaderName];


        public static Filter SimpleGradientTrailFilter => Filters.Scene["LunarVeil:SimpleGradientTrail"];

        private static void RegisterMiscShader(string name, string path, string pass)
        {
            Asset<Effect> miscShader = Assets.Request<Effect>(path, AssetRequestMode.ImmediateLoad);
            var miscShaderData = new MiscShaderData(miscShader, pass);
            GameShaders.Misc[name] = miscShaderData;
        }




        public static void LoadShaders()
        {
            /*
            Asset<Effect> glowingDustShader = Assets.Request<Effect>("Assets/Effects/GlowingDust");
            GameShaders.Misc[CrystalMoonUtils.GlowingDustShader] = new MiscShaderData(glowingDustShader, "GlowingDustPass");

            Asset<Effect> miscShader = Assets.Request<Effect>("Assets/Effects/Clouds", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:Clouds"] = new MiscShaderData(miscShader, "ScreenPass");

            Asset<Effect> miscShader2 = Assets.Request<Effect>("Assets/Effects/CloudsFront", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:CloudsFront"] = new MiscShaderData(miscShader2, "ScreenPass");

            Asset<Effect> miscShader3 = Assets.Request<Effect>("Assets/Effects/NightClouds", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:NightClouds"] = new MiscShaderData(miscShader3, "ScreenPass");

            Asset<Effect> miscShader4 = Assets.Request<Effect>("Assets/Effects/CloudsDesert", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:CloudsDesert"] = new MiscShaderData(miscShader4, "ScreenPass");

            Asset<Effect> miscShader5 = Assets.Request<Effect>("Assets/Effects/CloudsDesertNight", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:CloudsDesertNight"] = new MiscShaderData(miscShader5, "ScreenPass");

            var miscShader6 = new Ref<Effect>(CrystalMoon.Instance.Assets.Request<Effect>("Assets/Effects/Water", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["LunarVeil:Water"] = new Filter(new ScreenShaderData(miscShader6, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:Water"].Load();

            var miscShader7 = new Ref<Effect>(CrystalMoon.Instance.Assets.Request<Effect>("Assets/Effects/WaterBasic", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["LunarVeil:WaterBasic"] = new Filter(new ScreenShaderData(miscShader7, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:WaterBasic"].Load();


            var miscShader8 = new Ref<Effect>(CrystalMoon.Instance.Assets.Request<Effect>("Assets/Effects/Lava", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["LunarVeil:Lava"] = new Filter(new ScreenShaderData(miscShader8, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:Lava"].Load();

            Asset<Effect> gradient = Assets.Request<Effect>("Assets/Effects/Gradient", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:Gradient"] = new MiscShaderData(gradient, "ScreenPass");

            Asset<Effect> gradientTrail = Assets.Request<Effect>("Assets/Effects/SimpleGradientTrail", AssetRequestMode.ImmediateLoad);
            Filters.Scene["LunarVeil:SimpleGradientTrail"] = new Filter(new ScreenShaderData(gradientTrail, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:SimpleGradientTrail"].Load();

            SkyManager.Instance["LunarVeil:CloudySky"] = new CloudySky();
            SkyManager.Instance["LunarVeil:CloudySky"].Load();
            Filters.Scene["LunarVeil:CloudySky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);


            SkyManager.Instance["LunarVeil:DesertSky"] = new DesertSky();
            SkyManager.Instance["LunarVeil:DesertSky"].Load();
            Filters.Scene["LunarVeil:DesertSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);



            Asset<Effect> CometTrail = Assets.Request<Effect>("Assets/Effects/CometTrail", AssetRequestMode.ImmediateLoad);
            Filters.Scene["LunarVeil:CometTrail"] = new Filter(new ScreenShaderData(CometTrail, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:CometTrail"].Load();



            Asset<Effect> FadingTrail = Assets.Request<Effect>("Assets/Effects/FadingTrail", AssetRequestMode.ImmediateLoad);
            Filters.Scene["LunarVeil:FadingTrail"] = new Filter(new ScreenShaderData(FadingTrail, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:FadingTrail"].Load();




           



            Ref<Effect> BasicTrailRef = new(Assets.Request<Effect>("Assets/Effects/BasicTrailShader", AssetRequestMode.ImmediateLoad).Value);
            Ref<Effect> LightningTrailRef = new(Assets.Request<Effect>("Assets/Effects/LightningTrailShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc[CrystalMoonUtil.VampKnives_Basic_Trail] = new MiscShaderData(BasicTrailRef, "TrailPass");
            GameShaders.Misc[CrystalMoonUtil.VampKnives_Lightning_Trail] = new MiscShaderData(LightningTrailRef, "TrailPass");
             */
            /*
            Asset<Effect> shader2 = ModContent.Request<Effect>("Stellamod/Trails/SilhouetteShader", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc[CrystalMoonUtils.Silhouette_Shader] = new MiscShaderData(new Ref<Effect>(shader2.Value), "SilhouettePass");
            
            Ref<Effect> genericLaserShader = new(Assets.Request<Effect>("Assets/Effects/GenericLaserShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc[CrystalMoonUtil.VampKnives_Generic_Laser_Shader] = new MiscShaderData(genericLaserShader, "TrailPass");

            Ref<Effect> LightBeamVertexShader = new(Assets.Request<Effect>("Assets/Effects/LightBeamVertexShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc[CrystalMoonUtil.VampKnives_Light_Beam_Vertex_Shader] = new MiscShaderData(LightBeamVertexShader, "TrailPass");

            Ref<Effect> ArtemisLaserShader = new(Assets.Request<Effect>("Assets/Effects/ArtemisLaserShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc[CrystalMoonUtil.VampKnives_Artemis_Laser_Shader] = new MiscShaderData(ArtemisLaserShader, "TrailPass");

            Ref<Effect> shadowflameShader = new(Assets.Request<Effect>("Assets/Effects/Shadowflame", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc[CrystalMoonUtil.VampKnives_Fire] = new MiscShaderData(shadowflameShader, "TrailPass");

            Ref<Effect> whiteflameShader = new(Assets.Request<Effect>("Assets/Effects/Whiteflame", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc[CrystalMoonUtil.LunarVeilFireWhiteShader] = new MiscShaderData(whiteflameShader, "TrailPass");


            Ref<Effect> GenericLaserShader = new(Assets.Request<Effect>("Assets/Effects/LaserShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["LunarVeil:LaserShader"] = new MiscShaderData(GenericLaserShader, "TrailPass");

            //Filters.Scene["LunarVeil:LaserShader"].Load();
            /*
            Asset<Effect> blackShader = Assets.Request<Effect>("Effects/Black");
            Filters.Scene[CrystalMoonUtils.Screen_Black] = new Filter(new ScreenShaderData(blackShader, "BlackPass"), EffectPriority.Medium);

            Asset<Effect> tintShader = Assets.Request<Effect>("Effects/Tint");
            Filters.Scene[CrystalMoonUtils.Screen_Tint] = new Filter(new ScreenShaderData(tintShader, "ScreenPass"), EffectPriority.Medium);

            Asset<Effect> distortionShader = Assets.Request<Effect>("Effects/NormalDistortion");
            Filters.Scene[CrystalMoonUtils.Screen_NormalDistortion] = new Filter(new ScreenShaderData(distortionShader, "ScreenPass"), EffectPriority.Medium);

            Asset<Effect> vignetteShader = Assets.Request<Effect>("Effects/Vignette");
            Filters.Scene[CrystalMoonUtils.Screen_Vignette] = new Filter(new ScreenShaderData(vignetteShader, "ScreenPass"), EffectPriority.Medium);
       
            //White Flame Pixel Shader
            RegisterMiscShader(FireWhitePixelShaderName, "Effects/WhiteflamePixelShader", "TrailPass");

            //Test Shader (For Testing)
            RegisterMiscShader(TestPixelShaderName, "Effects/TestShader", "PixelPass");

            //Sil Shader
            RegisterMiscShader(SilShaderName, "Effects/SilShader", "PixelPass");

            //Distortion Shader
            RegisterMiscShader(DistortionShaderName, "Effects/NormalDistortion", "ScreenPass");
                 */

        }

        public static void LoadOrderedLoadables()
        {
            _loadCache = new List<IOrderedLoadable>();
            foreach (Type type in CrystalMoon.Instance.Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetInterfaces().Contains(typeof(IOrderedLoadable)))
                {
                    object instance = Activator.CreateInstance(type);
                    _loadCache.Add(instance as IOrderedLoadable);
                }

                _loadCache.Sort((n, t) => n.Priority.CompareTo(t.Priority));
            }

            for (int k = 0; k < _loadCache.Count; k++)
            {
                _loadCache[k].Load();
            }
        }

        public static void UnloadOrderedLoadables()
        {
            if (_loadCache != null)
            {
                foreach (IOrderedLoadable loadable in _loadCache)
                {
                    loadable.Unload();
                }

                _loadCache = null;
            }
            else
            {
             //   Logger.Warn("load cache was null, IOrderedLoadable's may not have been unloaded...");
            }
        }
        public static int ParticleType<T>() where T : Particle => ModContent.GetInstance<T>()?.Type ?? 0;

        public static bool OnScreen(Vector2 pos) => pos.X > -16 && pos.X < Main.screenWidth + 16 && pos.Y > -16 && pos.Y < Main.screenHeight + 16;

        public static bool OnScreen(Rectangle rect) => rect.Intersects(new Rectangle(0, 0, Main.screenWidth, Main.screenHeight));

        public static bool OnScreen(Vector2 pos, Vector2 size) => OnScreen(new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y));

        public static void DrawTrail(GraphicsDevice device, Action draw
        , BlendState blendState = null, SamplerState samplerState = null, RasterizerState rasterizerState = null)
        {
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            BlendState originalBlendState = Main.graphics.GraphicsDevice.BlendState;
            SamplerState originalSamplerState = Main.graphics.GraphicsDevice.SamplerStates[0];

            device.BlendState = blendState ?? originalBlendState;
            device.SamplerStates[0] = samplerState ?? originalSamplerState;
            device.RasterizerState = rasterizerState ?? originalState;

            draw();

            device.RasterizerState = originalState;
            device.BlendState = originalBlendState;
            device.SamplerStates[0] = originalSamplerState;
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public static Matrix GetTransfromMaxrix()
        {
            Vector3 screenPosition = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0);
            Matrix world = Matrix.CreateTranslation(-screenPosition);
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            return world * view * projection;
        }

        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.X, vector2.Y, 0);
        }

        public static float EllipticalEase(float rotation, float halfShortAxis, float halfLongAxis)
        {
            float halfFocalLength2 = (halfLongAxis * halfLongAxis) - (halfShortAxis * halfShortAxis);
            float cosX = MathF.Cos(rotation);
            return (halfLongAxis * halfShortAxis) / MathF.Sqrt(halfLongAxis * halfLongAxis - halfFocalLength2 * cosX * cosX);
        }
    }
}

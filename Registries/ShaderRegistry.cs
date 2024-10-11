using CrystalMoon.Systems.Skies;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.Graphics.Shaders;

namespace CrystalMoon.Registries
{
    internal class ShaderRegistry
    {
        public static AssetRepository Assets => CrystalMoon.Instance.Assets;
        public static MiscShaderData SimpleTrailEffect => GameShaders.Misc["CrystalMoon:SimpleTrail"];
        public static MiscShaderData SimpleGradientTrailEffect => GameShaders.Misc["CrystalMoon:SimpleGradientTrail"];
      
        //Trail Shaders
        public static MiscShaderData MagicTrailHex => GameShaders.Misc["CrystalMoon:MagicHex"];
        public static MiscShaderData MagicTrailRadiance => GameShaders.Misc["CrystalMoon:MagicRadiance"];
        public static MiscShaderData MagicTrailRadianceOutline => GameShaders.Misc["CrystalMoon:MagicRadianceOutline"];
        public static MiscShaderData MagicTrailBloodlet => GameShaders.Misc["CrystalMoon:MagicBloodlet"];
        public static MiscShaderData MagicTrailWater => GameShaders.Misc["CrystalMoon:MagicSparkleWater"];
        public static MiscShaderData MagicTrailDeeya => GameShaders.Misc["CrystalMoon:MagicDeeya"];
        public static MiscShaderData MagicTrailPhantasmal => GameShaders.Misc["CrystalMoon:MagicPhantasmal"];
        public static MiscShaderData MagicTrailGuut => GameShaders.Misc["CrystalMoon:MagicGuut"];
        public static MiscShaderData MagicTrailNatural => GameShaders.Misc["CrystalMoon:MagicNatural"];
        public static MiscShaderData MagicTrailNormal => GameShaders.Misc["CrystalMoon:MagicNormal"];


        //Pixel Shaders
        public static MiscShaderData DyePixelMagicFire => GameShaders.Misc["CrystalMoon:DyeFirePixel"];
        public static MiscShaderData PixelMagicFire => GameShaders.Misc["CrystalMoon:FirePixel"];
        public static MiscShaderData PixelMagicPhantasmal => GameShaders.Misc["CrystalMoon:PixelMagicPhantasmal"];
        public static MiscShaderData PixelMagicNormal => GameShaders.Misc["CrystalMoon:PixelMagicNormal"];
        public static MiscShaderData PixelMagicBloodlet => GameShaders.Misc["CrystalMoon:PixelMagicBloodlet"];
        public static MiscShaderData PixelMagicHex => GameShaders.Misc["CrystalMoon:PixelMagicHex"];
        public static MiscShaderData PixelMagicNatural => GameShaders.Misc["CrystalMoon:PixelMagicNatural"];
        public static MiscShaderData PixelMagicWater => GameShaders.Misc["CrystalMoon:PixelMagicSparkleWater"];
        public static MiscShaderData PixelMagicGuut => GameShaders.Misc["CrystalMoon:PixelMagicGuut"];

        public static MiscShaderData CloudsShader => GameShaders.Misc["CrystalMoon:Clouds"];
        public static MiscShaderData FogShader => GameShaders.Misc["CrystalMoon:Fog"];
        public static MiscShaderData CloudsFrontShader => GameShaders.Misc["CrystalMoon:CloudsFront"];
        public static MiscShaderData NightCloudsShader => GameShaders.Misc["CrystalMoon:NightClouds"];
        public static MiscShaderData CloudsDesertShader => GameShaders.Misc["CrystalMoon:CloudsDesert"];
        public static MiscShaderData CloudsDesertNightShader => GameShaders.Misc["CrystalMoon:CloudsDesertNight"];

        public static Filter WaterFilter => Filters.Scene["CrystalMoon:Water"];
        public static Filter WaterBasicFilter => Filters.Scene["CrystalMoon:WaterBasic"];
        public static Filter LavaFilter => Filters.Scene["CrystalMoon:Lava"];
        public static MiscShaderData GradientShader => GameShaders.Misc["CrystalMoon:Gradient"];
        public static Filter CloudySkyFilter => Filters.Scene["CrystalMoon:CloudySky"];
        public static Filter DesertSkyFilter => Filters.Scene["CrystalMoon:DesertSky"];
        private static void RegisterMiscShader(string name, string pass)
        {
            string assetPath = $"Assets/Effects/{name}";
            Asset<Effect> miscShader = Assets.Request<Effect>(assetPath, AssetRequestMode.ImmediateLoad);
            GameShaders.Misc[$"CrystalMoon:{name}"] = new MiscShaderData(miscShader, pass);
        }

        public static void LoadShaders()
        {
            RegisterMiscShader("SimpleTrail", "PrimitivesPass");
            RegisterMiscShader("Clouds", "ScreenPass");
            RegisterMiscShader("CloudsFront", "ScreenPass");
            RegisterMiscShader("NightClouds", "ScreenPass");
            RegisterMiscShader("CloudsDesert", "ScreenPass");
            RegisterMiscShader("CloudsDesertNight", "ScreenPass");
            RegisterMiscShader("Gradient", "ScreenPass");
            RegisterMiscShader("SimpleGradientTrail", "PrimitivesPass");

            //Magic Shaders
            RegisterMiscShader("MagicHex", "PrimitivesPass");
            RegisterMiscShader("MagicRadiance", "PrimitivesPass");
            RegisterMiscShader("MagicRadianceOutline", "PrimitivesPass");
            RegisterMiscShader("MagicBloodlet", "PrimitivesPass");
            RegisterMiscShader("MagicSparkleWater", "PrimitivesPass");
            RegisterMiscShader("MagicDeeya", "PrimitivesPass");
            RegisterMiscShader("MagicPhantasmal", "PrimitivesPass");
            RegisterMiscShader("MagicGuut", "PrimitivesPass");
            RegisterMiscShader("MagicNatural", "PrimitivesPass");
            RegisterMiscShader("MagicNormal", "PrimitivesPass");
  
            RegisterMiscShader("Fog", "PixelPass");


            //Magic Pixel
            RegisterMiscShader("FirePixel", "PixelPass");
            RegisterMiscShader("DyeFirePixel", "PixelPass");
            RegisterMiscShader("PixelMagicPhantasmal", "PixelPass");
            RegisterMiscShader("PixelMagicNormal", "PixelPass");
            RegisterMiscShader("PixelMagicBloodlet", "PixelPass");
            RegisterMiscShader("PixelMagicSparkleWater", "PixelPass");
    
            RegisterMiscShader("PixelMagicGuut", "PixelPass");
            RegisterMiscShader("PixelMagicNatural", "PixelPass");
            RegisterMiscShader("PixelMagicHex", "PixelPass");

            var miscShader9 = new Ref<Effect>(CrystalMoon.Instance.Assets.Request<Effect>("Assets/Effects/Water", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CrystalMoon:Water"] = new Filter(new ScreenShaderData(miscShader9, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["CrystalMoon:Water"].Load();

            var miscShader7 = new Ref<Effect>(CrystalMoon.Instance.Assets.Request<Effect>("Assets/Effects/WaterBasic", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CrystalMoon:WaterBasic"] = new Filter(new ScreenShaderData(miscShader7, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["CrystalMoon:WaterBasic"].Load();


            var miscShader8 = new Ref<Effect>(CrystalMoon.Instance.Assets.Request<Effect>("Assets/Effects/Lava", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CrystalMoon:Lava"] = new Filter(new ScreenShaderData(miscShader8, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["CrystalMoon:Lava"].Load();

            Asset<Effect> gradientTrail = Assets.Request<Effect>("Assets/Effects/SimpleGradientTrail", AssetRequestMode.ImmediateLoad);
            Filters.Scene["CrystalMoon:SimpleGradientTrail"] = new Filter(new ScreenShaderData(gradientTrail, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["CrystalMoon:SimpleGradientTrail"].Load();

            SkyManager.Instance["CrystalMoon:CloudySky"] = new CloudySky();
            SkyManager.Instance["CrystalMoon:CloudySky"].Load();
            Filters.Scene["CrystalMoon:CloudySky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);


            SkyManager.Instance["CrystalMoon:DesertSky"] = new DesertSky();
            SkyManager.Instance["CrystalMoon:DesertSky"].Load();
            Filters.Scene["CrystalMoon:DesertSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);
        }
    }
}

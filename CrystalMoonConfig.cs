using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CrystalMoon
{
    internal class CrystalMoonClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Visuals")]
        [DefaultValue(true)] // This sets the configs default value. // Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool SkiesToggle;

        [DefaultValue(true)] // This sets the configs default value.// Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool WatersToggle;

        [DefaultValue(false)]
        public bool LowDetailShadersToggle;

        [DefaultValue(true)] // This sets the configs default value.// Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        [ReloadRequired]
        public bool VanillaRespritesToggle;

        [DefaultValue(true)] // This sets the configs default value.// Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        [ReloadRequired]
        public bool VanillaUIRespritesToggle;

        [Header("Camera")]
        [DefaultValue(true)]
        public bool ShakeToggle;
        
        [Range(0f, 100f)]
        public float CameraSmoothness = 100;

        [Header("UI")]
        [Range(0f, 100f)]
        public float StaminaMeterX = 50;
        [Range(0f, 100f)]
        public float StaminaMeterY = 3;

        [Range(0f, 100f)]
        public float EnchantMenuX = 50;
        [Range(0f, 100f)]
        public float EnchantMenuY = 50;
    }
}

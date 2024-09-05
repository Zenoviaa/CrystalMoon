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

        [DefaultValue(true)] // This sets the configs default value.// Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        [ReloadRequired]
        public bool VanillaRespritesToggle;

        [DefaultValue(true)] // This sets the configs default value.// Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        [ReloadRequired]
        public bool VanillaUIRespritesToggle;

        [Header("Screenshake")]
        [DefaultValue(true)]
        public bool ShakeToggle;

        [Header("UI")]
        [Range(0f, 100f)]
        public float StaminaMeterX;
        [Range(0f, 100f)]
        public float StaminaMeterY;
    }
}

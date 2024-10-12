using Terraria.Audio;

namespace CrystalMoon.Registries
{
    internal static class SoundRegistry
    {
        public static string RootAssetPath => "CrystalMoon/Assets/Sounds/";

        //Example how to add
        // public static SoundStyle StormDragonLightningRain => new SoundStyle($"{RootAssetPath}StormDragon_LightingZap");

        public static SoundStyle CrystalHit1 => new SoundStyle($"{RootAssetPath}CrystalHit1");
        public static SoundStyle NSwordSlash1 => new SoundStyle($"{RootAssetPath}NormalSwordSlash1");

        public static SoundStyle HeavySwordSlash1 => new SoundStyle($"{RootAssetPath}HeavySwordSlash1");

        public static SoundStyle HeavySwordSlash2 => new SoundStyle($"{RootAssetPath}HeavySwordSlash2");

        public static SoundStyle NSwordSlash2 => new SoundStyle($"{RootAssetPath}NormalSwordSlash2");

        public static SoundStyle NSwordHit1 => new SoundStyle($"{RootAssetPath}NormalSwordHit1");

        public static SoundStyle NSwordSpin1 => new SoundStyle($"{RootAssetPath}SwordSpin1");

        public static SoundStyle CurveSwordSlash1 => new SoundStyle($"{RootAssetPath}CurveSwordSlash1");

        public static SoundStyle SpearSlash1 => new SoundStyle($"{RootAssetPath}SpearSlash1");

        public static SoundStyle SpearSlash2 => new SoundStyle($"{RootAssetPath}SpearSlash2");

        public static SoundStyle SpearHit1 => new SoundStyle($"{RootAssetPath}SpearHit1");


        /*
        public static SoundStyle BowCharge => new SoundStyle($"{RootAssetPath}BowCharge",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle BowShoot => new SoundStyle($"{RootAssetPath}BowShot",
            variantSuffixesStart: 1,
            numVariants: 2);
        */

    }
}

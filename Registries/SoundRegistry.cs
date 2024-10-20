using Terraria.Audio;

namespace CrystalMoon.Registries
{
    internal static class SoundRegistry
    {
        public static string RootAssetPath => "CrystalMoon/Assets/Sounds/";

        //Example how to add
        // public static SoundStyle StormDragonLightningRain => new SoundStyle($"{RootAssetPath}StormDragon_LightingZap");

        public static SoundStyle CrystalHit1 => new SoundStyle($"{RootAssetPath}CrystalHit1");

        public static SoundStyle FireHit1 => new SoundStyle($"{RootAssetPath}Cinder");
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

        public static SoundStyle HammerHit1 => new SoundStyle($"{RootAssetPath}HammerHit1");

        public static SoundStyle HammerHit2 => new SoundStyle($"{RootAssetPath}HammerHit2");

        public static SoundStyle HammerSmash1 => new SoundStyle($"{RootAssetPath}HammerSmash1");

        public static SoundStyle HammerSmash2 => new SoundStyle($"{RootAssetPath}HammerSmash2");

        public static SoundStyle HammerSmash3 => new SoundStyle($"{RootAssetPath}HammerSmash3");

        public static SoundStyle HammerSmashLightning1 => new SoundStyle($"{RootAssetPath}HammerSmashLightning1");

        public static SoundStyle MagicShockwaveExplosion => new SoundStyle($"{RootAssetPath}MagicalShockwave");

        public static SoundStyle FireShockwaveExplosion => new SoundStyle($"{RootAssetPath}FireShockwave");

        public static SoundStyle GaseousShockwaveExplosion => new SoundStyle($"{RootAssetPath}GaseousShockwave");

        public static SoundStyle LightShockwaveExplosion => new SoundStyle($"{RootAssetPath}MagicalShockwave2");

        public static SoundStyle GunShootLight1 => new SoundStyle($"{RootAssetPath}Gunshot1");

        public static SoundStyle MusicChord1 => new SoundStyle($"{RootAssetPath}MusicChord1");

        public static SoundStyle HeavyExplosion1 => new SoundStyle($"{RootAssetPath}HeavyExplosion1");

        public static SoundStyle GunShootHeavy1 => new SoundStyle($"{RootAssetPath}GunShot2");

        public static SoundStyle BowHolding => new SoundStyle($"{RootAssetPath}BowHolding");

        public static SoundStyle CastTickSummon1 => new SoundStyle($"{RootAssetPath}Summoner1");

        public static SoundStyle MotorcycleSlash1 => new SoundStyle($"{RootAssetPath}MotoSlash");

        public static SoundStyle MotorcycleSlash2 => new SoundStyle($"{RootAssetPath}MotoSlash2");

        public static SoundStyle MotorcycleDrive => new SoundStyle($"{RootAssetPath}MotoMot");

        public static SoundStyle RadianceCast1 => new SoundStyle($"{RootAssetPath}RadianceCast1");

        public static SoundStyle RadianceHit1 => new SoundStyle($"{RootAssetPath}RadianceHit1");

        public static SoundStyle WindCast => new SoundStyle($"{RootAssetPath}WindCast",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle WindHit => new SoundStyle($"{RootAssetPath}WindHit",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle UvilisCast => new SoundStyle($"{RootAssetPath}UvilisCast",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle UvilisHit => new SoundStyle($"{RootAssetPath}UvilisHit",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle PrismaticHit => new SoundStyle($"{RootAssetPath}PrismaticHit1");

        public static SoundStyle BloodletCast => new SoundStyle($"{RootAssetPath}BloodletCast1");

        public static SoundStyle BloodletHit => new SoundStyle($"{RootAssetPath}BloodletHit",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle AutomationCast => new SoundStyle($"{RootAssetPath}AutomationCast1");
        public static SoundStyle AutomationHit => new SoundStyle($"{RootAssetPath}AutomationHit",
            variantSuffixesStart: 1,
            numVariants: 2);

        public static SoundStyle NatureHit => new SoundStyle($"{RootAssetPath}NaturalHit",
          variantSuffixesStart: 1,
          numVariants: 2);
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

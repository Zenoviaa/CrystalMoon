using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.Players
{
    public class BowPlayer : ModPlayer
    {
        public int ArrowArmorPenetration;
        public int PracticeTargetCounter = 0;

        public override void ResetEffects()
        {

            ArrowArmorPenetration = 0;
        }

        public override void PostUpdateEquips()
        {
            if (!Player.GetModPlayer<LunarPlayer>().PracticeTarget) PracticeTargetCounter = 0;

            base.PostUpdateEquips();
        }
    }
}
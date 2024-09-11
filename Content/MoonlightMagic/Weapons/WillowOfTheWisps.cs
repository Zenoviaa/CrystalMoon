﻿using CrystalMoon.Content.MoonlightMagic.Forms;

namespace CrystalMoon.Content.MoonlightMagic.Weapons
{
    internal class WillowOfTheWisps : BaseStaff
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 12;
            Item.shootSpeed = 5;
            Size = 24;
            TrailLength = 55;
            Form = FormRegistry.Crescent.Value;
        }


        public override int GetNormalSlotCount()
        {
            return 3;
        }

        public override int GetTimedSlotCount()
        {
            return 1;
        }
    }
}

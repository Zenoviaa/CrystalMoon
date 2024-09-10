﻿using CrystalMoon.Content.MoonlightMagic.Forms;

namespace CrystalMoon.Content.MoonlightMagic.Weapons
{
    internal class MothwingStaff : BaseStaff
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 9;
            Item.shootSpeed = 13;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Size = 8;
            TrailLength = 38;
            Form = FormRegistry.Fairy.Value;
        }


        public override int GetNormalSlotCount()
        {
            return 2;
        }

        public override int GetTimedSlotCount()
        {
            return 1;
        }
    }
}

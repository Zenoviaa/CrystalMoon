using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrystalMoon.Systems.Players
{


    public class MoonPlayer : ModPlayer
    {

        
        public override void PostUpdate()
        {

            if (Player.ZoneSnow)
            {

                //Update Rain
                Main.raining = true;

                //That way, if it is already raining, it won't be overriden
                //And if it is not raining, it'll just be permanent until you leave the biome
                if (Main.rainTime <= 2)
                    Main.rainTime = 2;


                Main.maxRaining = 0.8f;

                Main.maxRain = 140;

            }

        }
    }
}
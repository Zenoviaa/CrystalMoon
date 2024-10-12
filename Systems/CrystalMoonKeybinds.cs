using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CrystalMoon.Systems
{
    internal class CrystalMoonKeybinds : ModSystem
    {
        public static ModKeybind DashKeybind { get; private set; }
        public override void Load()
        {
            // Register keybinds            
            DashKeybind = KeybindLoader.RegisterKeybind(Mod, "Dash", "F");
        }
    }
}

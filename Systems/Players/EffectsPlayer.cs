using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CrystalMoon.Systems.Players
{


    public class EffectsPlayer : ModPlayer
    {
        public float screenFlash;
        //private float screenFlashSpeed = 0.05f;
        //private Vector2? screenFlashCenter;
        private float shakeDrama;

        public void ShakeAtPosition(Vector2 position, float distance, float strength)
        {
            CrystalMoonClientConfig config = ModContent.GetInstance<CrystalMoonClientConfig>();
            if (!config.ShakeToggle)
                return;
            shakeDrama = strength * (1f - base.Player.Center.Distance(position) / distance) * 0.5f;
        }


        public override void ModifyScreenPosition()
        {
           
            if (shakeDrama > 0.5f)
            {
                shakeDrama *= 0.92f;
                Vector2 shake = new Vector2(Main.rand.NextFloat(shakeDrama), Main.rand.NextFloat(shakeDrama));
                Main.screenPosition += shake;
            }
        }
    }
}
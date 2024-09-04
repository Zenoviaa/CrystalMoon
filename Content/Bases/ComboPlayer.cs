using Terraria.ModLoader;

namespace LunarVeil.Content.Bases
{
    internal class ComboPlayer : ModPlayer
    {
        private int _comboWaitTimer;
        public int ComboCounter;
        public int ComboWaitTime;
        public int ComboDirection = 1;
        public override void PostUpdate()
        {
            base.PostUpdate();
            _comboWaitTimer++;
            if(_comboWaitTimer >= ComboWaitTime)
            {
                ComboCounter = 0;
                _comboWaitTimer = 0;
            }
        }

        public void IncreaseCombo(int maxCombo)
        {
            _comboWaitTimer = 0;
            ComboCounter++;
            if(ComboCounter >= maxCombo)
            {
                ComboCounter = 0;
            }
            ComboDirection = -ComboDirection;
        }
    }
}

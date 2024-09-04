using Terraria.ModLoader;

namespace CrystalMoon.Systems.Players
{

    public class ForegroundPlayer : ModPlayer
    {
        /*
        public override void PreUpdate()
        {


            if (Main.hasFocus)
                AddForegroundOrBackground();
        }

        private void AddForegroundOrBackground()
        {
            if (Player.ZoneSnow)
            {
                int leafFGChance = Starstrike.SpawnChance(Player);
                if (leafFGChance != -1 && Main.rand.NextBool(leafFGChance))
                {
                    bool spawnForegroundItem = true;
                    bool spawnOnPlayerLayer = true;
                    Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 2f), (int)(Main.screenWidth * 2f)), Main.screenHeight * 0.52f);
                    ForegroundHelper.AddItem(new Starstrike(pos), spawnForegroundItem, spawnOnPlayerLayer);
                }



                int SnowFGChance = Snowstrike.SpawnChance(Player);
                if (SnowFGChance != -1 && Main.rand.NextBool(SnowFGChance))
                {
                    bool spawnForegroundItem = true;
                    bool spawnOnPlayerLayer = true;
                    Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 2f), (int)(Main.screenWidth * 2f)), Main.screenHeight * 0.52f);
                    ForegroundHelper.AddItem(new Snowstrike(pos), spawnForegroundItem, spawnOnPlayerLayer);
                }
            }

            if (Main._shouldUseWindyDayMusic && !Player.ZoneDesert)
            {
                int leafFGChance = Cherryblossom.SpawnChance(Player);
                if (leafFGChance != -1 && Main.rand.NextBool(leafFGChance))
                {
                    bool spawnForegroundItem = true;
                    bool spawnOnPlayerLayer = true;
                    Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 2f), (int)(Main.screenWidth * 2f)), Main.screenHeight * 0.52f);
                    ForegroundHelper.AddItem(new Cherryblossom(pos), spawnForegroundItem, spawnOnPlayerLayer);
                }




            }

            if (Main.raining && Player.ZoneForest)
            {
                int leafFGChance = Cherryblossom.SpawnChance(Player);
                if (leafFGChance != -1 && Main.rand.NextBool(leafFGChance))
                {
                    bool spawnForegroundItem = true;
                    bool spawnOnPlayerLayer = true;
                    Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 2f), (int)(Main.screenWidth * 2f)), Main.screenHeight * 0.52f);
                    ForegroundHelper.AddItem(new Cherryblossom(pos), spawnForegroundItem, spawnOnPlayerLayer);
                }




            }

            if ((Player.ZoneDesert))
            {
                int leafFGChance = Sandstrike.SpawnChance(Player);
                if (leafFGChance != -1 && Main.rand.NextBool(leafFGChance))
                {
                    bool spawnForegroundItem = true;
                    bool spawnOnPlayerLayer = true;
                    Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 2f), (int)(Main.screenWidth * 2f)), Main.screenHeight * 0.52f);
                    ForegroundHelper.AddItem(new Sandstrike(pos), spawnForegroundItem, spawnOnPlayerLayer);
                }




            }

        }*/
    }
}
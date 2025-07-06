using System;

namespace WaffleStock
{
    public static class Constants
    {
        public static string HighScore = nameof(HighScore);
        public static string TotalFishedFishes = nameof(TotalFishedFishes);
        public static string TotalPointsScored = nameof(TotalPointsScored);
        public static string TotalTrashesHit = nameof(TotalTrashesHit);
        public static string TotalLostFishes = nameof(TotalLostFishes);
        public static string MaxFishedFishes = nameof(MaxFishedFishes);
        public static string MaxPointScored = nameof(MaxPointScored);
        public static string TotalTrashesCleaned = nameof(TotalTrashesCleaned);
        public static string TotalEatenFishes = nameof(TotalEatenFishes);
        public static string TotalGamesPlayed = nameof(TotalGamesPlayed);
        public static string LongestSession = nameof(LongestSession);
        public static string TotalTimePlayed = nameof(TotalTimePlayed);

        public static string AchievementsFolder = "res://Game/Assets/Achievements/";

        // /!\ WARNING /!\ Add new fishes/trashes/biomes at the end of enums to avoid messing the underlying numbering
        // /!\ WARNING /!\ Do NOT arrange them alpabetically because the '.tres' are only keeping track of the integer

        public static string FishesFolder = "res://Game/Assets/Fishes/";

        public enum Fishes
        {
            AnguilleFish,
            ArrowFish,
            GreenFish,
            RedFish,
            SardineFish,
            SwordFish,
            YellowFish,
            SharkFish,
            SerpentFish,
            JellyFish,
            SeaUrchin,
            BlueFish,
            ClownFish,
        }

        public static string TrashesFolder = "res://Game/Assets/Trashes/";

        public enum Trashes
        {
            Can,
            Shoe,
            Tire
        }

        public static string BiomesFolder = "res://Game/Assets/Biomes/";

        public enum Biomes
        {
            Beach,
            WreckDeck,
            WreckInside,
            Abyss,
            CoralReef,
            GreatBlue,
            OceanCurrent
        }
    }
}

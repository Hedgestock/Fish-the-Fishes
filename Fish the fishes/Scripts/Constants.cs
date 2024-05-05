using System;

namespace Godot.Fish_the_fishes.Scripts
{
    public static class Constants
    {
        public static string TotalFishedFishes = nameof(TotalFishedFishes);
        public static string TotalPointsScored = nameof(TotalPointsScored);
        public static string TotalTrashesHit = nameof(TotalTrashesHit);
        public static string TotalLostFishes = nameof(TotalLostFishes);
        public static string MaxFishedFishes = nameof(MaxFishedFishes);
        public static string MaxPointScored = nameof(MaxPointScored);
        public static string TotalTrashesCleaned = nameof(TotalTrashesCleaned);
        public static string TotalEatenFishes = nameof(TotalEatenFishes);


        public static string FishesFolder = "res://Fish the fishes/Assets/Fishes/";
        public enum Fishes
        {
            AnguilleFish,
            ArrowFish,
            GreenFish,
            RedFish,
            SardineFish,
            SwordFish,
            YellowFish
        }

        public static string TrashesFolder = "res://Fish the fishes/Assets/Trashes/";

        public enum Trashes
        {
            Can,
            Shoe,
            Tire
        }

        public static string BiomesFolder = "res://Fish the fishes/Assets/Biomes/";

        public enum Biomes
        {
            Beach,
            WreckDeck,
            WreckInside,
            Abyss,
            CoralReef,
            GreatBlue,
            OceanCurrent,
            _Test,
            _TestFollowup
        }
    }
}

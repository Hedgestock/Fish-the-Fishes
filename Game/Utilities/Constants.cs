using System;
using System.Collections.Generic;

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
            AnguilleFish    = 0,
            ArrowFish       = 1,
            BlueFish        = 11,
            BombFish        = 22,
            BossGiantSquid  = 19,
            BossWhale       = 20,
            BubbleFish      = 17,
            ClownFish       = 12,
            CurrentFish     = 18,
            GreenFish       = 2,
            GuppyFish       = 14,
            JellyFish       = 9,
            LakeFish        = 15,
            LanternFish     = 23,
            ParrotFish      = 13,
            PoisonFish      = 21,
            RedFish         = 3,
            RiverFish       = 16,
            SardineFish     = 4,
            SeaUrchin       = 10,
            SerpentFish     = 8,
            SharkFish       = 7,
            SwordFish       = 5,
            YellowFish      = 6,
        }
        // Highest 23, next 24

        public static string TrashesFolder = "res://Game/Assets/Trashes/";

        public enum Trashes
        {
            Can  = 0,
            Shoe = 1,
            Tire = 2,
        }
        // Highest 2, next 3

        public static string BiomesFolder = "res://Game/Assets/Biomes/";

        public enum Biomes
        {
            Abyss           = 3,
            Beach           = 0,
            CoralReef       = 4,
            GreatBlue       = 5,
            Lake            = 7,
            OceanCurrent    = 6,
            WreckDeck       = 1,
            WreckInside     = 2,
        }
        // Highest 7, next 8

        public static string EquipmentsFolder = "res://Game/Assets/Player/Equipment/";

        public static Dictionary<EquipmentPiece.Type, Dictionary<string, string>> EquipmentList = new()
        {
            [EquipmentPiece.Type.Hook] = new()
            {
                ["StandardHook"] = "uid://caai7o1q2a1n0",
                ["HugeHook"] = "uid://ogoejx5sl264",
                ["HarpoonHook"] = "uid://bv5hyshljxn2u",
                ["FastHook"] = "uid://dy1dv6tfxqajl",
                ["ControlHook"] = "uid://cghnipcqvmc3k",
            },
            [EquipmentPiece.Type.Line] = new()
            {
                ["StandardLine"] = "uid://r57nubtmjtlg",
            },
            [EquipmentPiece.Type.Weight] = new() {
                ["Cork"] = "uid://ti5hjckxy2yf",
                ["Lead"] = "uid://c1xqky60o4qx1",
            },
            [EquipmentPiece.Type.Attractor] = new() { },
        };
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace WaffleStock
{
    public class UserData
    {
        #region serializable instance
        private static UserData _instance = null;
        public Dictionary<string, long> _statistics { get; set; }
        public Dictionary<string, EquipmentStatus> _equipments { get; set; }

        public Dictionary<string, DateTime> _achievements { get; set; }
        public Dictionary<string, FishCompendiumEntry> _fishCompendium { get; set; }
        public Dictionary<string, TrashCompendiumEntry> _trashCompendium { get; set; }
        public Dictionary<string, BiomeCompendiumEntry> _biomeCompendium { get; set; }

        public UserData()
        {
            if (_instance != null)
                return;
            _statistics = new();
            _achievements = new();
            _equipments = new();
            _fishCompendium = new();
            _trashCompendium = new();
            _biomeCompendium = new();

            _instance = this;
        }
        #endregion

        public static Dictionary<string, DateTime> Achievements { get { return _instance._achievements; } set { _instance._achievements = value; } }
        public static Dictionary<string, EquipmentStatus> Equipments { get { return _instance._equipments; } set { _instance._equipments = value; } }
        public static Dictionary<string, FishCompendiumEntry> FishCompendium { get { return _instance._fishCompendium; } set { _instance._fishCompendium = value; } }
        public static Dictionary<string, TrashCompendiumEntry> TrashCompendium { get { return _instance._trashCompendium; } set { _instance._trashCompendium = value; } }
        public static Dictionary<string, BiomeCompendiumEntry> BiomeCompendium { get { return _instance._biomeCompendium; } set { _instance._biomeCompendium = value; } }

        public static void Reset()
        {
            _instance = null;
            new UserData();
        }
        public static string Serialize()
        {
            return JsonSerializer.Serialize(_instance);
        }

        public static bool Deserialize(string json)
        {
            try
            {
                _instance = JsonSerializer.Deserialize<UserData>(json);
                PropertyInfo[] properties = typeof(UserData).GetProperties();

                // This makes sure that we recover from corrupted data where any property is set to `null`
                // by replacing it with a new empty object.
                foreach (PropertyInfo property in properties)
                {
                    if (typeof(UserData).GetProperty(property.Name).GetValue(_instance) == null)
                    {
                        typeof(UserData).GetProperty(property.Name).SetValue(_instance, property.PropertyType.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>()));
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr(e);
                return false;
            }
        }

        public enum StatCategory
        {
            Scratch,
            Competitive,
            Casual
        }

        #region helper methods
        public static long GetStatistic(StatCategory category, Game.Mode mode, string statName)
        {
            return _instance._statistics.GetValueOrDefault($"{category}/{mode}/{statName}");
        }

        public static void SetHighStat(string statName, long stat)
        {
            if (GameManager.Mode <= Game.Mode.Training) return;
            _setHighStat(StatCategory.Scratch, Game.Mode.AllModes, statName, stat);
            _setHighStat(StatCategory.Scratch, GameManager.Mode, statName, stat);
            _setHighStat(UserSettings.CompetitiveMode ? StatCategory.Competitive : StatCategory.Casual, Game.Mode.AllModes, statName, stat);
            _setHighStat(UserSettings.CompetitiveMode ? StatCategory.Competitive : StatCategory.Casual, GameManager.Mode, statName, stat);
        }

        private static void _setHighStat(StatCategory category, Game.Mode mode, string statName, long stat)
        {
            long currentStat = _instance._statistics.GetValueOrDefault($"{category}/{mode}/{statName}");
            if (currentStat >= stat) return;
            _instance._statistics[$"{category}/{mode}/{statName}"] = stat;
        }

        public static void IncrementStatistic(string statName, long incr = 1)
        {
            if (GameManager.Mode <= Game.Mode.Training) return;
            _incrementStatistic(StatCategory.Scratch, Game.Mode.AllModes, statName, incr);
            _incrementStatistic(StatCategory.Scratch, GameManager.Mode, statName, incr);
            _incrementStatistic(UserSettings.CompetitiveMode ? StatCategory.Competitive : StatCategory.Casual, Game.Mode.AllModes, statName, incr);
            _incrementStatistic(UserSettings.CompetitiveMode ? StatCategory.Competitive : StatCategory.Casual, GameManager.Mode, statName, incr);
        }


        private static void _incrementStatistic(StatCategory category, Game.Mode mode, string statName, long incr)
        {
            long currentStat = _instance._statistics.GetValueOrDefault($"{category}/{mode}/{statName}");
            _instance._statistics[$"{category}/{mode}/{statName}"] = currentStat + incr;
        }

        #endregion

        #region helper classes
        public class EquipmentStatus
        {
            public EquipmentPiece.Type Type { get; set; }
            public bool IsEquipped { get; set; }
            public EquipmentStatus(EquipmentPiece.Type type, bool isEquipped = false)
            {
                Type = type;
                IsEquipped = isEquipped;
            }
        }

        public abstract class CompendiumEntry
        {
            public long Seen { get; set; }
        }

        public class FishCompendiumEntry : CompendiumEntry
        {
            public long Caught { get; set; }
            public float MaxSize { get; set; }
            public float MinSize { get; set; }
            public FishCompendiumEntry()
            {
                Caught = 0;
                Seen = 1;
                MinSize = -1;
                MaxSize = -1;
            }
        }

        public class TrashCompendiumEntry : CompendiumEntry
        {
            public long Hit { get; set; }
            public long Cleaned { get; set; }
            public TrashCompendiumEntry()
            {
                Hit = 0;
                Cleaned = 0;
                Seen = 1;
            }
        }

        public class BiomeCompendiumEntry : CompendiumEntry
        {
            public BiomeCompendiumEntry()
            {
                Seen = 1;
            }
        }
        #endregion
    }
}

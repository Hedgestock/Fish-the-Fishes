using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Godot.Fish_the_fishes.Scripts
{
    public class UserData
    {
        #region serializable instance
        private static UserData _instance = null;

        public Dictionary<string, uint> _competitiveScores { get; set; }
        public Dictionary<string, uint> _casualScores { get; set; }
        public Dictionary<string, uint> _statistics { get; set; }
        public Dictionary<string, FishCompendiumEntry> _fishCompendium { get; set; }
        public Dictionary<string, TrashCompendiumEntry> _trashCompendium { get; set; }

        public UserData()
        {
            if (_instance != null)
                return;
            _competitiveScores = new Dictionary<string, uint>();
            _casualScores = new Dictionary<string, uint>();
            _statistics = new Dictionary<string, uint>();
            _fishCompendium = new Dictionary<string, FishCompendiumEntry>();
            _trashCompendium = new Dictionary<string, TrashCompendiumEntry>();

            _instance = this;
        }
        #endregion

        public static Dictionary<string, uint> CompetitiveScores { get {return _instance._competitiveScores; } set { _instance._competitiveScores = value; } }
        public static Dictionary<string, uint> CasualScores { get {return _instance._casualScores; } set { _instance._casualScores = value; } }
        public static Dictionary<string, uint> Statistics { get {return _instance._statistics; } set { _instance._statistics = value; } }
        public static Dictionary<string, FishCompendiumEntry> FishCompendium { get {return _instance._fishCompendium; } set { _instance._fishCompendium = value; } }
        public static Dictionary<string, TrashCompendiumEntry> TrashCompendium { get {return _instance._trashCompendium; } set { _instance._trashCompendium = value; } }

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
                        typeof(UserData).GetProperty(property.Name).SetValue(_instance, property.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { }));
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

        #region helper classes
        public class FishCompendiumEntry
        {
            public uint Caught { get; set; }
            public uint Seen { get; set; }
            public FishCompendiumEntry()
            {
                Caught = 0;
                Seen = 1;
            }
        }

        public class TrashCompendiumEntry
        {
            public uint Hit { get; set; }
            public uint Cleaned { get; set; }
            public uint Seen { get; set; }
            public TrashCompendiumEntry()
            {
                Hit = 0;
                Cleaned = 0;
                Seen = 1;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace Godot.Fish_the_fishes.Scripts
{
    public class UserData
    {
        private static UserData _instance = null;

        public static UserData Instance
        {
            get
            {
                if (_instance == null) _instance = new UserData();
                return _instance;
            }
        }

        public Dictionary<string, uint> CompetitiveScores { get; set; }
        public Dictionary<string, uint> CasualScores { get; set; }
        public Dictionary<string, uint> Statistics { get; set; }
        public Dictionary<string, CompendiumEntry> Compendium { get; set; }

        public UserData()
        {
            CompetitiveScores = new Dictionary<string, uint>();
            CasualScores = new Dictionary<string, uint>();
            Statistics = new Dictionary<string, uint>();
            Compendium = new Dictionary<string, CompendiumEntry>();
        }

        public static void Reset()
        {
            _instance = null;
        }
        public static string Serialize()
        {
            return JsonSerializer.Serialize(Instance);
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

        public class CompendiumEntry
        {
            public uint Caught { get; set; }
            public uint Seen { get; set; }
            public CompendiumEntry()
            {
                Caught = 0;
                Seen = 1;
            }
        }
    }
}

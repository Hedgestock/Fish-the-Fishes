﻿using System.Collections.Generic;
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

        public UserData()
        {
            CompetitiveScores = new Dictionary<string, uint>();
            CasualScores = new Dictionary<string, uint>();
            Statistics = new Dictionary<string, uint>();
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
                return true;
            }
            catch (System.Exception e)
            {
                GD.PrintErr(e);
                return false;
            }
        }
    }
}
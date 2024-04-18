using System.Collections.Generic;
using System.Text.Json;

namespace Godot.Fish_the_fishes.Scripts
{
    public static class UserData
    {
        private class SerializableData
        {
            public Dictionary<string, uint> CompetitiveScores { get; set; }
            public Dictionary<string, uint> CasualScores { get; set; }
            public Dictionary<string, dynamic> Statistics { get; set; }
        }

        public static Dictionary<string, uint> CompetitiveScores = new Dictionary<string, uint>()
        {
            {"Classic", 0},
            {"TimeAttack", 0}
        };

        public static Dictionary<string, uint> CasualScores = new Dictionary<string, uint>()
        {
            {"Classic", 0},
            {"TimeAttack", 0}
        };
        public static Dictionary<string, dynamic> Statistics = new Dictionary<string, dynamic>()
        {
            {"TotalFishedFishes", 0},
            {"MaxFishedFishes", 0}
        };

        public static string Serialize()
        {
            var tmp = new SerializableData() { CompetitiveScores = CompetitiveScores, CasualScores = CasualScores, Statistics = Statistics };
            return JsonSerializer.Serialize(tmp);
        }

        public static void Deserialize(string json)
        {
            SerializableData tmp = JsonSerializer.Deserialize<SerializableData>(json);
            CompetitiveScores = tmp.CompetitiveScores;
            CasualScores = tmp.CasualScores;
            Statistics = tmp.Statistics;
        }
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Game;


namespace WaffleStock
{
    public static class Scoring
    {
        public static int ScoringFunction(int num, int b = 3)
        {
            if (num <= 0) return 0;
            return (int)(num * MathF.Log(num, b) + 1);
        }

        public static int ClassicScore(List<IFishable> scoredFishes, bool free = true)
        {
            float score = 0;

            //GD.Print("Classic scoring");

            //foreach (var item in scoredFishes)
            //{
            //    GD.Print(item.GetType());
            //}
            foreach (Fish fish in scoredFishes)
            {
                score += fish.Value;
            }
            score = ScoringFunction((int)Math.Ceiling(score));
            foreach (Fish fish in scoredFishes)
            {
                if (fish.IsNegative)
                {
                    score = -score;
                    break;
                }
            }

            UserData.SetHighStat(Constants.MaxFishedFishes, scoredFishes.Count);
            UserData.IncrementStatistic(Constants.TotalFishedFishes, scoredFishes.Count);

            GameManager.CurrentBiomeCatches += scoredFishes.Count;

            foreach (Fish fish in scoredFishes)
            {
                UpdateFishCompendium(fish);
                score *= fish.Multiplier;
                if (fish is IPowerup powerup) powerup.Activate();
                if (free) fish.QueueFree();
            }
            AchievementsManager.OnFishFished();

            UserData.SetHighStat(Constants.MaxPointScored, (long)score);
            UserData.IncrementStatistic(Constants.TotalPointsScored, (long)score);

            return (int)score;
        }

        public static int TargetScore(List<IFishable> scoredFishes)
        {
            int score = scoredFishes.Any(thing => thing.GetType().Name == GameManager.Target.ToString()) ? 1 : 0;

            UserData.SetHighStat(Constants.MaxFishedFishes, (uint)scoredFishes.Count);
            UserData.IncrementStatistic(Constants.TotalFishedFishes, (uint)scoredFishes.Count);

            UserData.SetHighStat(Constants.MaxPointScored, (uint)score);
            UserData.IncrementStatistic(Constants.TotalPointsScored, (uint)score);

            foreach (Fish fish in scoredFishes)
            {
                UpdateFishCompendium(fish);
                if (fish is IPowerup powerup) powerup.Activate();
                fish.QueueFree();
            }
            AchievementsManager.OnFishFished();

            return score;
        }

        private static void UpdateFishCompendium(Fish fish)
        {
            if (GameManager.Mode <= Game.Mode.Training) return;

            string fishTypeName = fish.GetType().Name;
            if (!UserData.FishCompendium.ContainsKey(fishTypeName))
                return;

            UserData.FishCompendium[fishTypeName].Caught++;
            if (UserData.FishCompendium[fishTypeName].MaxSize < 0)
            {
                UserData.FishCompendium[fishTypeName].MaxSize = fish.ActualSize;
                UserData.FishCompendium[fishTypeName].MinSize = fish.ActualSize;
            }
            else if (UserData.FishCompendium[fishTypeName].MaxSize < fish.ActualSize)
            {
                UserData.FishCompendium[fishTypeName].MaxSize = fish.ActualSize;
            }
            else if (UserData.FishCompendium[fishTypeName].MinSize > fish.ActualSize)
            {
                UserData.FishCompendium[fishTypeName].MinSize = fish.ActualSize;
            }
        }
    }
}

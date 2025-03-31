using System;


namespace Godot.Fish_the_fishes.Scripts
{
    public interface IAchievable : IDescriptible
    {
        public enum CheckTiming
        {
            OnGameStart,
            OnGameEnd,
            OnFishFished,
            OnPointScored,
            OnHit
        }
        
        public CheckTiming Timing { get; }

        public bool Predicate();

    }
}

using System;


namespace Wafflestock
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

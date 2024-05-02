using System;

namespace Godot.Fish_the_fishes.Scripts
{
    public interface IFishable
    {
        public bool IsCaught { get; set;  }
        public IFishable GetCaughtBy(IFisher by);
    }
}

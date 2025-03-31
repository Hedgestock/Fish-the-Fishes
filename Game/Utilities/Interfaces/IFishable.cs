using System;

namespace Godot.FishTheFishes
{
    public interface IFishable
    {
        public bool IsCaught { get; set;  }
        public IFishable GetCaughtBy(IFisher by);
    }
}

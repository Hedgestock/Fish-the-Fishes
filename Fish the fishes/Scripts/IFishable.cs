using System;

namespace Godot.Fish_the_fishes.Scripts
{
    public interface IFishable
    {
        public IFishable GetCaughtBy(IFisher by);
    }
}

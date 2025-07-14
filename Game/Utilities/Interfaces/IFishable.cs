using Godot;
using System;

namespace WaffleStock
{
    public interface IFishable
    {
        public bool IsInDisplay { get; set; }
        public bool IsCaught { get; set; }
        public IFishable GetCaughtBy(IFisher by);
    }
}

using System;

namespace Wafflestock
{
    public interface IFishable
    {
        public bool IsCaught { get; set;  }
        public IFishable GetCaughtBy(IFisher by);
    }
}

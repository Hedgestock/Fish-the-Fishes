using System;

namespace WaffleStock
{
    public interface IFishable
    {
        public bool IsCaught { get; set;  }
        public IFishable GetCaughtBy(IFisher by);
    }
}

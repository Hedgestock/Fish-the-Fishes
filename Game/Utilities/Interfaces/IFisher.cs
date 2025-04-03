using System;
using System.Collections.Generic;

namespace WaffleStock
{
    public interface IFisher
    {
        List<IFishable> FishedThings { get; }
    }
}

using System;
using System.Collections.Generic;

namespace Wafflestock
{
    public interface IFisher
    {
        List<IFishable> FishedThings { get; }
    }
}

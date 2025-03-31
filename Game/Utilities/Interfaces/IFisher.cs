using System;
using System.Collections.Generic;

namespace Godot.FishTheFishes
{
    public interface IFisher
    {
        List<IFishable> FishedThings { get; }
    }
}

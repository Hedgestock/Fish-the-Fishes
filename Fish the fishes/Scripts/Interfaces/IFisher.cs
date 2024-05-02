using System;
using System.Collections.Generic;

namespace Godot.Fish_the_fishes.Scripts
{
    public interface IFisher
    {
        List<IFishable> FishedThings { get; }
    }
}

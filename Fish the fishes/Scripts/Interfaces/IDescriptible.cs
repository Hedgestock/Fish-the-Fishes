using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godot.Fish_the_fishes.Scripts.Interfaces
{
    internal interface IDescriptible
    {
        public static string CompendiumName { get; }
        public static abstract string CompendiumDescription { get; }
    }
}

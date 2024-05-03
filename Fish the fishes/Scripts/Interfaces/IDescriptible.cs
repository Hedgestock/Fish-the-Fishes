using System;

namespace Godot.Fish_the_fishes.Scripts
{
    internal interface IDescriptible
    {
        public static string CompendiumName { get; }
        public static abstract string CompendiumDescription { get; }
    }
}

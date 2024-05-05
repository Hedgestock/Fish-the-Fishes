using System;


namespace Godot.Fish_the_fishes.Scripts
{
    internal interface IDescriptible
    {
        string CompendiumName { get; set; }
        string CompendiumDescription { get; set; }
    }
}

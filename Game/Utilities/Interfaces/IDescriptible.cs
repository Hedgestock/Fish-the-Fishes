using System;


namespace Godot.FishTheFishes
{
    public interface IDescriptible
    {
        public string CompendiumName { get; set; }
        public string CompendiumDescription { get; set; }
    }
}

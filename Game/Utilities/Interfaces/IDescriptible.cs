using System;


namespace Wafflestock
{
    public interface IDescriptible
    {
        public string CompendiumName { get; set; }
        public string CompendiumDescription { get; set; }
    }
}

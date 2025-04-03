using System;


namespace WaffleStock
{
    public interface IDescriptible
    {
        public string CompendiumName { get; set; }
        public string CompendiumDescription { get; set; }
    }
}

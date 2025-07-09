using System;
using System.Collections.Generic;
using System.Linq;

namespace WaffleStock
{
    public interface IFisher
    {
        public List<IFishable> FishedThings { get; }

        public List<IFishable> FlattenFishedThings(List<IFishable> fishedThings)
        {
            List<IFishable> scoredFishes = new(fishedThings);

            foreach (IFisher fisher in fishedThings.OfType<IFisher>())
            {
                scoredFishes.AddRange(FlattenFishedThings(fisher.FishedThings));
            }

            return scoredFishes;
        }
    }
}

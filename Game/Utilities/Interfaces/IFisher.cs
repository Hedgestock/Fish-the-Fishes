using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaffleStock
{
    public interface IFisher
    {
        public List<IFishable> FishedThings()
        {
            return (this as Node).GetChildren().OfType<IFishable>().ToList();
        }

        public List<IFishable> FlattennedFishedThings()
        {
            return FlattenFishedThings(FishedThings());
        }

        private List<IFishable> FlattenFishedThings(List<IFishable> fishedThings)
        {
            List<IFishable> fishes = new(fishedThings);

            foreach (IFisher fisher in fishedThings.OfType<IFisher>())
            {
                fishes.AddRange(FlattenFishedThings(fisher.FishedThings()));
            }

            return fishes;
        }
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


namespace WaffleStock
{
    public partial class WeightedItem<ItemType> : Resource
    {
        [Export]
        public uint Weight = 100;
        public ItemType Item;

        public static ItemType ChooseFrom(IEnumerable<WeightedItem<ItemType>> list)
        {
            return GetFrom(list).Item;
        }

        public static WeightedItem<ItemType> GetFrom(IEnumerable<WeightedItem<ItemType>> list)
        {
            if (!list.Any()) throw new ArgumentException("Weighted list cannot be empty", nameof(list));

            uint index = GD.Randi() % GetTotalWeight(list);

            uint currentWeight = 0;

            foreach (var weightedItem in list)
            {
                currentWeight += weightedItem.Weight;
                if (currentWeight > index)
                    return weightedItem;
            }

            // This mostly happens when all the weight are set to 0 and GetTotalWeight() returns 1
            return list.ToArray()[0];
        }


        public static uint GetTotalWeight(IEnumerable<WeightedItem<ItemType>> list)
        {
            uint totalWeight = 0;

            foreach (var item in list)
            {
                totalWeight += item.Weight;
            }

            // Cheeky hack to avoid dividing by zero in case of only 0 weights
            return totalWeight != 0 ? totalWeight : 1;
        }

    }
}

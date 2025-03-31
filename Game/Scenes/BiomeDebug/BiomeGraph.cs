using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class BiomeGraph : GraphEdit
{
    [Export]
    PackedScene BiomeNode;

    private Dictionary<string, BiomeGraphNode> VisitedBiomes = new Dictionary<string, BiomeGraphNode>();

    public override void _Ready()
    {
        base._Ready();

        TraverseBiomes(GameManager.StartingBiome.ResourceName);

        VisibilityChanged += async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(.02));
            ArrangeNodes();
        };
    }

    private void TraverseBiomes(string biomeName, BiomeGraphNode PreviousBiomeNode = null, int fromSlot = -1)
    {
        BiomeGraphNode thisBiomeNode;

        if (VisitedBiomes.TryGetValue(biomeName, out thisBiomeNode))
        {
            ConnectNode(PreviousBiomeNode.Name, fromSlot, thisBiomeNode.Name, 0);
            return;
        }

        Biome thisBiome = GD.Load<Biome>($"{Constants.BiomesFolder}/{biomeName}/{biomeName}.tres");

        thisBiomeNode = BiomeNode.Instantiate<BiomeGraphNode>();
        thisBiomeNode.Biome = thisBiome;
        AddChild(thisBiomeNode);
        thisBiomeNode.SetSlotEnabledLeft(0, true);
        //thisBiomeNode.SetSlotColorLeft(0, new Color("cyan"));

        if (PreviousBiomeNode != null)
        {
            thisBiomeNode.PositionOffset = PreviousBiomeNode.PositionOffset + new Vector2(500, 0);
            ConnectNode(PreviousBiomeNode.Name, fromSlot, thisBiomeNode.Name, 0);
        }

        VisitedBiomes.Add(biomeName, thisBiomeNode);

        int slotNumber = 1;

        foreach (WeightedBiome weightedBiome in thisBiome.FollowupBiomes)
        {
            thisBiomeNode.AddItem(weightedBiome, WeightedItem.GetTotalWeight(thisBiome.FollowupBiomes.ToArray()));
            thisBiomeNode.SetSlotEnabledRight(slotNumber, true);
            //thisBiomeNode.SetSlotColorRight(slotNumber, new Color("purple"));
            TraverseBiomes(weightedBiome.Biome.ToString(), thisBiomeNode, slotNumber - 1);
            slotNumber++;
        }
    }
}

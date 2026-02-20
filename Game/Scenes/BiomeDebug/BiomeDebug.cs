using Godot;
using WaffleStock;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BiomeDebug : CanvasLayer
{
    [Export]
    GraphEdit Graph;

    [Export]
    PackedScene BiomeNode;

    private Dictionary<string, BiomeGraphNode> VisitedBiomes = new Dictionary<string, BiomeGraphNode>();

    public override void _Ready()
    {
        base._Ready();

        TraverseBiomes(GameManager.StartingBiome.ResourceName);

        Graph.ArrangeNodes();
    }

    private void TraverseBiomes(string biomeName, BiomeGraphNode PreviousBiomeNode = null, int fromSlot = -1)
    {
        BiomeGraphNode thisBiomeNode;

        if (VisitedBiomes.TryGetValue(biomeName, out thisBiomeNode))
        {
            Graph.ConnectNode(PreviousBiomeNode.Name, fromSlot, thisBiomeNode.Name, 0);
            return;
        }

        Biome thisBiome = GD.Load<Biome>($"{Constants.BiomesFolder}/{biomeName}/{biomeName}.tres");

        thisBiomeNode = BiomeNode.Instantiate<BiomeGraphNode>();
        thisBiomeNode.Biome = thisBiome;
        Graph.AddChild(thisBiomeNode);
        thisBiomeNode.SetSlotEnabledLeft(0, true);
        //thisBiomeNode.SetSlotColorLeft(0, new Color("cyan"));

        if (PreviousBiomeNode != null)
        {
            thisBiomeNode.PositionOffset = PreviousBiomeNode.PositionOffset + new Vector2(500, 0);
            Graph.ConnectNode(PreviousBiomeNode.Name, fromSlot, thisBiomeNode.Name, 0);
        }

        VisitedBiomes.Add(biomeName, thisBiomeNode);

        int slotNumber = 1;

        foreach (WeightedBiome weightedBiome in thisBiome.FollowupBiomes)
        {
            thisBiomeNode.AddItem(weightedBiome, WeightedBiome.GetTotalWeight(thisBiome.FollowupBiomes));
            thisBiomeNode.SetSlotEnabledRight(slotNumber, true);
            //thisBiomeNode.SetSlotColorRight(slotNumber, new Color("purple"));
            TraverseBiomes(weightedBiome.Item.ToString(), thisBiomeNode, slotNumber - 1);
            slotNumber++;
        }
    }

    private Error Test(StringName from_node, int from_port, StringName to_node, int to_port)
    {
        GD.Print(from_node, " ", from_port, " ", to_node, " ", to_port);
        return Error.Ok;
    }
}

using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

public partial class BiomeDebug : CanvasLayer
{
    [Export]
    GraphEdit Graph;

    [Export]
    PackedScene BiomeNode;

    private HashSet<string> VisitedBiomes = new HashSet<string>();

    public override void _Ready()
    {
        base._Ready();

        TraverseBiomes(GameManager.StartingBiome.ResourceName);
    }

    private void TraverseBiomes(string biomeName, BiomeGraphNode PreviousBiomeNode = null, int fromSlot = -1)
    {
        if (VisitedBiomes.Contains(biomeName)) return;

        VisitedBiomes.Add(biomeName);

        Biome thisBiome = GD.Load<Biome>($"{Constants.BiomesFolder}/{biomeName}/{biomeName}.tres");
        
        BiomeGraphNode thisBiomeNode = CreateBiomeNode(thisBiome);
        if (PreviousBiomeNode != null) {
            thisBiomeNode.PositionOffset = PreviousBiomeNode.PositionOffset + new Vector2(500 ,0);
        } else
        {
            thisBiomeNode.PositionOffset += new Vector2(0, 150);
        }
        Graph.AddChild(thisBiomeNode);

        int slotNumber = 0;
        foreach (WeightedBiome weightedBiome in thisBiome.FollowupBiomes)
        {
            TraverseBiomes(weightedBiome.Biome.ToString(), thisBiomeNode, slotNumber);
            slotNumber++;
        }
    }

    private BiomeGraphNode CreateBiomeNode(Biome biome)
    {
        BiomeGraphNode node = BiomeNode.Instantiate<BiomeGraphNode>();

        node.Title = biome.ResourceName;

        foreach (WeightedFish weightedFish in biome.Fishes)
        {
            node.AddFish(weightedFish);
        }

        foreach (WeightedTrash weightedTrash in biome.Trashes)
        {
            node.AddTrash(weightedTrash);
        }

        foreach (WeightedBiome weightedBiome in biome.FollowupBiomes)
        {
            node.AddBiome(weightedBiome);
        }

        return node;
    }
}

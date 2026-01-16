using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class Test : CanvasLayer
{
    public override void _Ready()
    {
        base._Ready();

        GameManager.Biome.Fishes = new (Enum.GetValues<Constants.Fishes>().Select(f => new WeightedFish { Fish = f }));
        GameManager.Biome.Trashes = new (Enum.GetValues<Constants.Trashes>().Select(t => new WeightedTrash { Trash = t }));

    }

}

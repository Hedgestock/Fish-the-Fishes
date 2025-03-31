using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

[GlobalClass]
public partial class WeightedTrash : WeightedItem
{
    [Export]
    public Constants.Trashes Trash { get; set; }
}

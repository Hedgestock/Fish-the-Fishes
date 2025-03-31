using Godot;
using Godot.FishTheFishes;
using System;

[GlobalClass]
public partial class WeightedTrash : WeightedItem
{
    [Export]
    public Constants.Trashes Trash { get; set; }
}

using Godot;
using Wafflestock;
using System;

[GlobalClass]
public partial class WeightedTrash : WeightedItem
{
    [Export]
    public Constants.Trashes Trash { get; set; }
}

using Godot;
using WaffleStock;
using System;

[GlobalClass]
public partial class WeightedTrash : WeightedItem
{
    [Export]
    public Constants.Trashes Trash { get; set; }
}

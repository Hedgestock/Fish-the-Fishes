using Godot;
using System;

[GlobalClass]
public partial class WeightedItem : Resource
{
    [Export]
    public PackedScene Item { get; set; }
    [Export]
    public uint Weight = 100;
}

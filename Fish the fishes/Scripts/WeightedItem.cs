using Godot;
using System;

[GlobalClass]
public partial class WeightedItem : Resource
{
    [Export]
    public GodotObject Item { get; set; }
    [Export]
    public uint Weight = 100;
}

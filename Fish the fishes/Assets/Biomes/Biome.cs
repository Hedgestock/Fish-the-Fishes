using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Biome : Resource
{
    [Export]
    public Texture2D Background;

    [Export]
    public Array<WeightedItem> Fishes;

    [Export]
    public Array<WeightedItem> Trashes;

    [Export]
    public Array<Biome> FollowupBiomes;


}

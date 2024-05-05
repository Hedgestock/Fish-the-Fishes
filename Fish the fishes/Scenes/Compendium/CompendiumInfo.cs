using Godot;
using System;

[GlobalClass]
public partial class CompendiumInfo : Resource
{
    [Export]
    public string CompendiumName { get; set; }
    [Export]
    public string CompendiumDescription { get; set; }
}

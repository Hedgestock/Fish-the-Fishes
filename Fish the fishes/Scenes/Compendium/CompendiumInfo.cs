using Godot;
using System;

[GlobalClass]
public partial class CompendiumInfo : Resource
{
    [Export]
    public string CompendiumName { get; set; }

    [Export(PropertyHint.MultilineText)]
    public string CompendiumDescription { get; set; }
}

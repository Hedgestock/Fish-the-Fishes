using Godot;
using System;

public partial class CompendiumEntry : PanelContainer
{

    [Export]
    protected Label CompendiumName;
    [Export]
    protected Label CompendiumDescription;
    [Export]
    protected Label NumberSeen;

    public string TypeString;
}

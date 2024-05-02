using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class CompendiumEntry : PanelContainer
{

    [Export]
    protected Label CompendiumName;
    [Export]
    protected Label CompendiumDescription;

    public string TypeString;

    protected Type EntryType;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (string.IsNullOrEmpty(TypeString)) return;

        EntryType = Type.GetType(TypeString);

        CompendiumName.Text = (string)EntryType.GetField(nameof(CompendiumName)).GetValue(EntryType);
    }
}

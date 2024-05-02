using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class CompendiumEntry : PanelContainer
{

    [Export]
    private Label CompendiumName;
    [Export]
    private Label CompendiumDescription;

    public string TypeString;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (string.IsNullOrEmpty(TypeString)) return;

        Type EntryType = Type.GetType(TypeString);

        CompendiumName.Text = (string)EntryType.GetField(nameof(CompendiumName)).GetValue(EntryType);
    }
}

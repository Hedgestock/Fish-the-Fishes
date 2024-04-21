using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;

public partial class FishCompendiumEntry : HBoxContainer
{

    [Export]
    private Label CompendiumName;
    [Export]
    private Label CompendiumDescription;
    [Export]
    private Label NumberFished;
    [Export]
    private Label NumberSeen;
    [Export]
    private AnimatedSprite2D AnimatedSprite;

    public string FishTypeString;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (string.IsNullOrEmpty(FishTypeString)) return;

        Type FishType = Type.GetType(FishTypeString);

        try
        {
            NumberSeen.Text = UserData.Instance.Compendium[FishTypeString].Seen.ToString();
            NumberFished.Text = UserData.Instance.Compendium[FishTypeString].Caught.ToString();

            CompendiumName.Text = (string)FishType.GetField(nameof(CompendiumName)).GetValue(FishType);
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
            GD.PrintErr("YOU SHOULD PROBABLY ADD A STATIC NAME AND DESCRIPTION TO THIS FISH !");
        }

        if (UserData.Instance.Compendium[FishTypeString].Caught > 0)
        {
            CompendiumDescription.Text = (string)FishType.GetField(nameof(CompendiumDescription)).GetValue(FishType);
        }
        else
        {
            AnimatedSprite.Modulate = new Color(0, 0, 0);
        }


        string ressourcePath = $"res://Fish the fishes/Assets/Fishes/{FishTypeString}/{FishTypeString}Animation.tres";

        if (FileAccess.FileExists(ressourcePath))
        {
            AnimatedSprite.SpriteFrames = GD.Load<SpriteFrames>(ressourcePath);
            AnimatedSprite.Animation = "alive";
            AnimatedSprite.Play();
            CallDeferred(MethodName.PlaceAnimatedSprite);
        }
        else
        {
            GD.PrintErr("No animation resource found at path: ", ressourcePath);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void PlaceAnimatedSprite()
    {
        AnimatedSprite.Position = new Vector2(Size.X / 2, Size.Y / 2);
    }
}

using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class FishCompendiumEntry : CompendiumEntry
{

    [Export]
    private Label NumberFished;
    [Export]
    private Label NumberSeen;
    [Export]
    private AnimatedSpriteForUI Placeholder;
    [Export]
    private HBoxContainer AnimationButtons;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        string resourcePath = $"{Constants.FishesFolder}{TypeString}/{TypeString}Animation.tres";

        Placeholder.SpriteFrames = GD.Load<SpriteFrames>(resourcePath);

        if (Placeholder.SpriteFrames != null)
        {
            Placeholder.Animation = "alive";
            Placeholder.Play();

            if (!UserData.FishCompendium.ContainsKey(TypeString))
            {
                Placeholder.Modulate = new Color(0, 0, 0);
                return;
            }
        }
        else
        {
            GD.PrintErr("No animation resource found at path: ", resourcePath);
        }

        NumberSeen.Text = UserData.FishCompendium[TypeString].Seen.ToString();
        NumberFished.Text = UserData.FishCompendium[TypeString].Caught.ToString();

        if (UserData.FishCompendium[TypeString].Caught > 0)
        {
            CompendiumDescription.Text = (string)EntryType.GetProperty(nameof(CompendiumDescription)).GetValue(EntryType);
            if (Placeholder.SpriteFrames.GetAnimationNames().Length > 1)
            {
                AnimationButtons.Show();
            }
        }
    }

    private void PreviousAnimation()
    {
        ChangeAnimation(-1);
    }

    private void NextAnimation()
    {
        ChangeAnimation(1);
    }

    private void ChangeAnimation(int step)
    {
        string[] animationNames = Placeholder.SpriteFrames.GetAnimationNames();
        int currentIndex = Array.FindIndex(animationNames, animation => animation == Placeholder.Animation);
        Placeholder.Animation = animationNames[Mathf.PosMod(currentIndex + step, animationNames.Length)];
    }
}

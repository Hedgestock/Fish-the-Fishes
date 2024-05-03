using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class TrashCompendiumEntry : CompendiumEntry
{

    [Export]
    private Label NumberCleaned;
    [Export]
    private Label NumberHit;
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

        string resourcePath = $"{Constants.TrashesFolder}{TypeString}/{TypeString}Animation.tres";

        Placeholder.SpriteFrames = GD.Load<SpriteFrames>(resourcePath);

        if (Placeholder.SpriteFrames != null)
        {
            Placeholder.Play();

            if (!UserData.TrashCompendium.ContainsKey(TypeString))
            {
                Placeholder.Modulate = new Color(0, 0, 0);
                return;
            }
        }
        else
        {
            GD.PrintErr("No animation resource found at path: ", resourcePath);
        }

        NumberSeen.Text = UserData.TrashCompendium[TypeString].Seen.ToString();
        NumberHit.Text = UserData.TrashCompendium[TypeString].Hit.ToString();
        NumberCleaned.Text = UserData.TrashCompendium[TypeString].Cleaned.ToString();
        CompendiumDescription.Text = (string)EntryType.GetProperty(nameof(CompendiumDescription)).GetValue(EntryType);
        if (Placeholder.SpriteFrames.GetAnimationNames().Length > 1)
        {
            AnimationButtons.Show();
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

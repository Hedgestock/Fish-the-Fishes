using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class AnimatedCompendiumEntry : CompendiumEntry
{
    [Export]
    private AnimatedSpriteForUI Placeholder;
    [Export]
    private HBoxContainer AnimationButtons;

    private string ResourcePath;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        ResourcePath = $"{EntryFolder}{EntryKey}Animation.tres";

        Placeholder.SpriteFrames = GD.Load<SpriteFrames>(ResourcePath);

        if (Placeholder.SpriteFrames != null)
        {
            Placeholder.Play();

            if ((EntryType == Compendium.EntryType.Trash && !UserData.TrashCompendium.ContainsKey(EntryKey))
                || (EntryType == Compendium.EntryType.Fish && !UserData.FishCompendium.ContainsKey(EntryKey)))
            {
                Placeholder.Modulate = new Color(0, 0, 0);
                return;
            }
        }
        else
        {
            GD.PrintErr("No animation resource found at path: ", ResourcePath);
        }
    }

    protected void ShowAnimationButtons()
    {
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

using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class FishCompendiumEntry : PanelContainer
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
    private AnimatedSpriteForUI Placeholder;
    [Export]
    private HBoxContainer AnimationButtons;

    public string FishTypeString;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (string.IsNullOrEmpty(FishTypeString)) return;

        Type FishType = Type.GetType(FishTypeString);

        string resourcePath = $"res://Fish the fishes/Assets/Fishes/{FishTypeString}/{FishTypeString}Animation.tres";

        Placeholder.SpriteFrames = GD.Load<SpriteFrames>(resourcePath);

        if (Placeholder.SpriteFrames != null)
        {
            Placeholder.Animation = "alive";
            Placeholder.Play();

            if (!UserData.Instance.FishCompendium.ContainsKey(FishTypeString))
            {
                Placeholder.Modulate = new Color(0, 0, 0);
                return;
            }
        }
        else
        {
            GD.PrintErr("No animation resource found at path: ", resourcePath);
        }


        NumberSeen.Text = UserData.Instance.FishCompendium[FishTypeString].Seen.ToString();
        NumberFished.Text = UserData.Instance.FishCompendium[FishTypeString].Caught.ToString();

        CompendiumName.Text = (string)FishType.GetField(nameof(CompendiumName)).GetValue(FishType);


        if (UserData.Instance.FishCompendium[FishTypeString].Caught > 0)
        {
            CompendiumDescription.Text = (string)FishType.GetField(nameof(CompendiumDescription)).GetValue(FishType);
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

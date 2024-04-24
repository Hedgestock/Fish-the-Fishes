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
    private BoxContainer Placeholder;
    [Export]
    private AnimatedSprite2D AnimatedSprite;
    [Export]
    private HBoxContainer AnimationButtons;

    public string FishTypeString;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (string.IsNullOrEmpty(FishTypeString)) return;

        Type FishType = Type.GetType(FishTypeString);

        string ressourcePath = $"res://Fish the fishes/Assets/Fishes/{FishTypeString}/{FishTypeString}Animation.tres";

        AnimatedSprite.SpriteFrames = GD.Load<SpriteFrames>(ressourcePath);

        if (AnimatedSprite.SpriteFrames != null)
        {
            AnimatedSprite.Animation = "alive";
            AnimatedSprite.Play();

            Placeholder.CustomMinimumSize = AnimatedSprite.SpriteFrames.GetFrameTexture(AnimatedSprite.Animation, 0).GetSize();
            CallDeferred(MethodName.PlaceAnimatedSprite);

            GetTree().Root.SizeChanged += PlaceAnimatedSprite;

            if (!UserData.Instance.Compendium.ContainsKey(FishTypeString))
            {
                AnimatedSprite.Modulate = new Color(0, 0, 0);
                return;
            }
        }
        else
        {
            GD.PrintErr("No animation resource found at path: ", ressourcePath);
        }


        NumberSeen.Text = UserData.Instance.Compendium[FishTypeString].Seen.ToString();
        NumberFished.Text = UserData.Instance.Compendium[FishTypeString].Caught.ToString();

        CompendiumName.Text = (string)FishType.GetField(nameof(CompendiumName)).GetValue(FishType);


        if (UserData.Instance.Compendium[FishTypeString].Caught > 0)
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
        string[] animationNames = AnimatedSprite.SpriteFrames.GetAnimationNames();
        int currentIndex = Array.FindIndex(animationNames, animation => animation == AnimatedSprite.Animation);
        AnimatedSprite.Animation = animationNames[Mathf.PosMod(currentIndex + step, animationNames.Length)];
    }

    private void PlaceAnimatedSprite()
    {
        AnimatedSprite.GlobalPosition = new Vector2(Placeholder.GlobalPosition.X + Placeholder.Size.X / 2, Placeholder.GlobalPosition.Y + Placeholder.Size.Y / 2);
    }
}

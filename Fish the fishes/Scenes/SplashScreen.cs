using Godot;
using System;

public partial class SplashScreen : ColorRect
{
    [Export]
    Label Wafflestock;
    [Export]
    VideoStreamPlayer Logo;

    public override void _Ready()
    {
        Visible = GameManager.PrevScene == "";

        if (!Visible) return;

        Wafflestock.Modulate = new Color("ffffff00");
        Logo.Play();
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Wafflestock, "modulate", new Color("ffffff"), .5);
        tween.TweenProperty(this, "modulate", new Color("ffffff"), 1);
        tween.TweenProperty(this, "modulate", new Color("ffffff00"), .5);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}

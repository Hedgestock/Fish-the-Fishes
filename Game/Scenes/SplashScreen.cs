using Godot;
using System;

public partial class SplashScreen : ColorRect
{
    [Export]
    Label WaffleStock;
    [Export]
    VideoStreamPlayer Logo;
    [Export]
    TextureRect LastFrame;
    [Export]
    AudioStreamPlayer FTF;

    public override void _Ready()
    {
        Visible = GameManager.PrevScene == "";

        if (!Visible) return;

        WaffleStock.Modulate = new Color("ffffff00");
        Logo.Play();
        //Logo.Connect(VideoStreamPlayer.SignalName.Finished, Callable.From(() => { LastFrame.Show(); Logo.Hide(); }));
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(WaffleStock, "modulate", new Color("ffffff"), .5);
        tween.TweenProperty(this, "modulate", new Color("ffffff"), 1);
        tween.TweenProperty(this, "modulate", new Color("ffffff00"), .5);
        tween.TweenCallback(Callable.From(() =>
            {
                QueueFree();
                FTF.Play();
            }));
    }
}

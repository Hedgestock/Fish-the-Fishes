using Godot;
using System;
using System.Reflection.Metadata;

public partial class Notification : PanelContainer
{
    [Export]
    public Label AchievementName;


    public void FadeAway()
    {
        Tween tweenOpacity = CreateTween();
        tweenOpacity.TweenProperty(this, "modulate:a", 0, 1);
        tweenOpacity.TweenCallback(Callable.From(QueueFree));
    }
}

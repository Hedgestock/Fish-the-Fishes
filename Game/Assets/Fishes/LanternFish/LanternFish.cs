using Godot;
using Godot.Collections;
using System;
using WaffleStock;

public partial class LanternFish : Fish
{
    [Export]
    public Array<Constants.Fishes> ImmuneToAttraction = new Array<Constants.Fishes>();

    [Export]
    private PackedScene Blood;

    [Export]
    Area2D AttractionBox;
    [Export]
    Area2D HitBox;
    [Export]
    CollisionShape2D MouthRange;

    [Export]
    Timer Timer;

    private void OnFishedLured(Node2D body)
    {
        if (!(body is Fish) || body == this || !MouthRange.Disabled || !IsActionable || FishListContains(ImmuneToAttraction, body.GetType())) return;

        Fish Food = body as Fish;

        if (Food.IsHuge) return;

        MouthRange.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        Sprite.Animation = "eating";
        Timer.Start();
    }

    private void OnFishEaten(Node2D body)
    {
        if (!(body is Fish) || body == this || !IsActionable) return;

        Fish Food = body as Fish;


        if (Food.IsHuge) return;

        Food.Kill();
        GpuParticles2D bleeding = Blood.Instantiate<GpuParticles2D>();
        bleeding.Emitting = true;
        bleeding.GlobalPosition = Food.GlobalPosition;
        GetParent().AddChild(bleeding);
        GetTree().CreateTimer(bleeding.Lifetime).Timeout += bleeding.QueueFree;

        Food.QueueFree();
    }

    private void OnFinishedEating()
    {
        Sprite.Animation = IsAlive ? "alive" : "dead";
        MouthRange.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
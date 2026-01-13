using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using WaffleStock;

public partial class LanternFish : Fish, IFisher
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

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    public override void _PhysicsProcess(double delta)
    {
        foreach (var prey in AttractionBox.GetOverlappingBodies().OfType<Fish>())
        {
            LureFish(prey);
        }
        base._PhysicsProcess(delta);
    }

    public override void Kill()
    {
        HitBox.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        AttractionBox.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        MouthRange.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        base.Kill();
    }

    private void OnFishedLured(Node2D body)
    {
        if (!(body is Fish) || body == this || !MouthRange.Disabled || !IsActionable || FishListContains(ImmuneToAttraction, body.GetType())) return;

        Fish Food = body as Fish;

        MouthRange.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        Sprite.Animation = "eating";
        Timer.Start();
    }

    private void OnFishEaten(Node2D body)
    {
        if (!(body is Fish) || body == this || !IsActionable || body is Boss) return;

        Fish Food = body as Fish;

        if (Food.IsHuge || !Food.GetCaughtBy(this)) return;

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

    private void OnAttractionAreaEntered(Node2D body)
    {
        if (!(body is Fish) || body == this || !IsActionable) return;

        LureFish(body as Fish);
    }

    private void LureFish(Fish prey)
    {
        if (!IsActionable || !prey.IsActionable || prey.IsHuge || FishListContains(ImmuneToAttraction, prey.GetType())) return;

        prey.Velocity = prey.GlobalPosition.DirectionTo(HitBox.GlobalPosition) * prey.MaxSpeed;
        prey.Rotation = prey.Velocity.Angle();
    }
}
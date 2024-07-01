using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

public partial class SharkFish : Fish, IFisher
{
    [ExportGroup("Attributes")]
    [Export]
    private CollisionShape2D HitBox;
    [Export]
    private GpuParticles2D Bubbles;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        // Here, we invert the Flip condition to get a point that's on the opposite side of the spawning Position
        Vector2 objective = new Vector2(!Flip ? GameManager.ScreenSize.X + 200 : -200, (float)GD.RandRange(0, GameManager.ScreenSize.Y));
        //Velocity = (objective - Position).Normalized() * ActualSpeed;
        Velocity = Vector2.Zero;

        GpuParticles2D indicator = (GpuParticles2D)Bubbles.Duplicate();
        indicator.Position = (objective - Position).Normalized() * 300;
        

        GetParent().AddChild(indicator);


        Rotation = (float)(Velocity.Angle() - (Flip? Mathf.Pi: 0));

        Bubbles.Amount = (int)ActualSpeed / 10;
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        CleanCurrentBehaviors();
        return base.GetCaughtBy(by);
    }

    public override void Kill()
    {
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        CleanCurrentBehaviors();
        base.Kill();
    }

    private void CleanCurrentBehaviors()
    {
        Bubbles.Emitting = false;
    }

    private void OnFishEaten(Node2D body)
    {
        if (!(body is Fish) || FishedThings.Contains(body as Fish) || body == this || !Actionable) return;

        Fish Food = body as Fish;

        //Food = Food.GetCaughtBy(this) as Fish;
        //Food.Kill();

        Food.QueueFree();
    }

    #region helpers
    private Vector2 GetDirectionTo(Fish target)
    {
        return target.GlobalPosition - GlobalPosition;
    }
    #endregion helpers
}

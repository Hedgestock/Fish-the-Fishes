using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

public partial class SharkFish : Fish, IFisher
{
    [ExportGroup("Attributes")]
    //[Export]
    //private GpuParticles2D Blood;
    [Export]
    private CollisionShape2D HitBox;
    //[Export]
    //private CpuParticles2D Bubbles;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    private SceneTreeTimer LaunchTimer = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Velocity = Vector2.Zero;

        //CpuParticles2D indicator = (CpuParticles2D)Bubbles.Duplicate();
        //indicator.ProcessMaterial = (Material)Bubbles.ProcessMaterial.Duplicate();
        //indicator.Position = GlobalPosition + (travelAxis * 250);
        //(indicator.ProcessMaterial as ParticleProcessMaterial).Gravity = new Vector3(travelAxis.X, travelAxis.Y, 0) * 500;
        //indicator.Gravity = new Vector2(travelAxis.X, travelAxis.Y) * 500;
        //GetParent().AddChild(indicator);

        //Bubbles.Amount = (int)ActualSpeed / 10;
        Hide();

        LaunchTimer = GetTree().CreateTimer(2);
        LaunchTimer.Timeout += () =>
        {
            //indicator.QueueFree();
            if (!IsInstanceValid(this)) return;
            Show();
            if (!IsActionable) return;
            Velocity = TravelAxis * ActualSpeed;
        };
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
        //Bubbles.Emitting = false;
    }

    private void OnFishEaten(Node2D body)
    {
        if (!(body is Fish) || FishedThings.Contains(body as Fish) || body == this || !IsActionable) return;

        Fish Food = body as Fish;

        //Food = Food.GetCaughtBy(this) as Fish;
        //Food.Kill();
        if (!Food.IsHuge)
        {
            //GpuParticles2D bleeding = (GpuParticles2D)Blood.Duplicate();
            //bleeding.Emitting = true;
            //bleeding.Position = Food.Position;
            //GetParent().AddChild(bleeding);
            //GetTree().CreateTimer(bleeding.Lifetime).Timeout += bleeding.QueueFree;

            Food.QueueFree();
        }
    }
}

using Godot;
using Godot.Collections;
using WaffleStock;
using System;
using System.Collections.Generic;

public partial class SharkFish : Fish, IFisher
{
    [Export]
    public Array<Constants.Fishes> CantFlee = new Array<Constants.Fishes>();

    [Export]
    private PackedScene Blood;
    [Export]
    private CollisionShape2D HitBox;
    [Export]
    private GpuParticles2D Bubbles;

    private SceneTreeTimer LaunchTimer = null;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Velocity = Vector2.Zero;

        if (IsInDisplay) return;

        GpuParticles2D indicator = (GpuParticles2D)Bubbles.Duplicate();
        indicator.ZIndex = 1;
        indicator.ProcessMaterial = (Material)Bubbles.ProcessMaterial.Duplicate();
        indicator.Position = GlobalPosition + (TravelAxis * 300);
        (indicator.ProcessMaterial as ParticleProcessMaterial).Gravity = new Vector3(TravelAxis.X, TravelAxis.Y, 0) * 500;
        GetParent().AddChild(indicator);

        Bubbles.Amount = (int)ActualSpeed / 4;
        GD.Print(Bubbles.Amount);

        if (UserSettings.Vibrations) Input.VibrateHandheld(500);

        FrightenFishes();

        Hide();

        LaunchTimer = GetTree().CreateTimer(2);
        LaunchTimer.Timeout += () =>
        {
            indicator.QueueFree();
            if (!IsInstanceValid(this)) return;
            Show();
            HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

            if (!IsActionable) return;
            Velocity = TravelAxis * ActualSpeed;
        };
    }

    public override void GetCaughtBy(IFisher fisher)
    {
        base.GetCaughtBy(fisher);

        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        CleanCurrentBehaviors();
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
        if (!(body is IFishable fishable) || body == this || !IsActionable
            || fishable.Escape(this))
            return;

        if (!(fishable is Fish fish))
        {
            fishable.GetCaughtBy(this);
            return;
        }

        fish.Kill();

        if (fish.IsHuge) return;

        fish.GetCaughtBy(this);

        GpuParticles2D bleeding = Blood.Instantiate<GpuParticles2D>();
        bleeding.Emitting = true;
        bleeding.GlobalPosition = fish.GlobalPosition + Velocity.Normalized() * 100;
        GetParent().AddChild(bleeding);
        GetTree().CreateTimer(bleeding.Lifetime).Timeout += bleeding.QueueFree;

        fish.QueueFree();
    }

    private void FrightenFishes(int i = 0, int limit = -1)
    {
        if (limit < 0) limit = GD.RandRange(8, 12);
        if (i >= limit) return;
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        if (FishListContains(CantFlee, fish.GetType()))
        {
            // If fish is invalid, we retry but still increment the count
            // to avoid infinite loop on badly configured biomes
            Callable.From<int, int>(FrightenFishes).CallDeferred(++i, limit);
            return;
        }
        // Otherwise we spawn it
        fish.Position = Position;
        fish.Flip = Flip;
        fish.ActualSpeed = fish.MaxSpeed * 1.5f;

        fish.TravelAxis = TravelAxis.Rotated((float)GD.RandRange(-.3, .3));

        // Spawn the fish by adding it to the main scene.
        GetParent().AddChild(fish);
        Callable.From<int, int>(FrightenFishes).CallDeferred(++i, limit);
    }
}

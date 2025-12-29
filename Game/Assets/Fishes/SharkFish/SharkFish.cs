using Godot;
using Godot.Collections;
using WaffleStock;
using System;
using System.Collections.Generic;

public partial class SharkFish : Fish, IFisher
{
    [Export]
    public Array<Constants.Fishes> CantFlee = new Array<Constants.Fishes>();

    //[Export]
    //private GpuParticles2D Blood;
    [Export]
    private CollisionShape2D HitBox;
    [Export]
    private GpuParticles2D Bubbles;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    private SceneTreeTimer LaunchTimer = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Velocity = Vector2.Zero;

        if (IsInDisplay) return;

        GpuParticles2D indicator = (GpuParticles2D)Bubbles.Duplicate();
        indicator.ProcessMaterial = (Material)Bubbles.ProcessMaterial.Duplicate();
        indicator.Position = GlobalPosition + (TravelAxis * 250);
        (indicator.ProcessMaterial as ParticleProcessMaterial).Gravity = new Vector3(TravelAxis.X, TravelAxis.Y, 0) * 500;
        GetParent().AddChild(indicator);

        Bubbles.Amount = (int)ActualSpeed / 3;

        if (UserSettings.Vibrations) Input.VibrateHandheld(500);

        FrightenFishes();

        Hide();

        LaunchTimer = GetTree().CreateTimer(2);
        LaunchTimer.Timeout += () =>
        {
            //indicator.QueueFree();
            if (!IsInstanceValid(this)) return;
            Show();
            HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

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
        Bubbles.Emitting = false;
    }

    private void OnFishEaten(Node2D body)
    {
        if (!(body is Fish) || FishedThings.Contains(body as Fish) || body == this || !IsActionable) return;

        Fish Food = body as Fish;

        //Food = Food.GetCaughtBy(this) as Fish;
        Food.Kill();
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

    private void FrightenFishes(int i = 0, int limit = -1)
    {
        if (limit < 0) limit = GD.RandRange(8, 12);
        if (i >= limit) return;
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        if (FishListContains(CantFlee, fish.GetType()))
        {
            // If fish is invalid, we retry
            Callable.From<int, int>(FrightenFishes).CallDeferred(i, limit);
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

        //for (int i = 0; i < GD.RandRange(8,12); i++)
        //{
        //    PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        //    Fish fish = FishScene.Instantiate<Fish>();
        //    if (FishListContains(CantFlee, fish.GetType()))
        //    {
        //        i--;
        //        continue;
        //    }
        //    fish.Position = Position;
        //    fish.Flip = Flip;
        //    fish.ActualSpeed = fish.MaxSpeed * 1.5f;

        //    fish.TravelAxis = TravelAxis.Rotated((float)GD.RandRange(-.3, .3));

        //    // Spawn the fish by adding it to the main scene.
        //    GetParent().AddChild(fish);
        //}
    }
}

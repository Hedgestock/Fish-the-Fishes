using Godot;
using Godot.Collections;
using WaffleStock;
using System;
using System.Linq;

public partial class ParrotFish : Fish
{
    [Export]
    public Array<Constants.Fishes> FoodTypes = new Array<Constants.Fishes>();

    [Export]
    private CollisionShape2D HitBox;

    public override void _Ready()
    {
        base._Ready();
        if (IsInDisplay) return;
        Fish[] targets = GetTree().GetNodesInGroup("Fishes").OfType<Fish>()
            .Where(target =>           
                FishListContains(FoodTypes, target.GetType()) && target.IsActionable
            ).ToArray();

        if (targets.Length == 0) return;

        Fish target = targets[(int)(GD.Randi() % targets.Length)];

        Velocity = GetDirectionTo(target).Normalized() * ActualSpeed;
        Rotation = (float)(Velocity.Angle() - (Flip ? Mathf.Pi : 0));
    }

    private Vector2 GetDirectionTo(Fish target)
    {
        return target.GlobalPosition - GlobalPosition;
    }

    private void OnFoodSecured(Node2D body)
    {
        if (!FishListContains(FoodTypes, body.GetType())) return;
        Velocity = Vector2.Zero;
        Fish food = body as Fish;
        // We "catch" it to prevent gravity taking over
        food.IsCaught = true;
        food.Kill();
        foreach (var hurtbox in food.GetChildren().OfType<CollisionShape2D>().Where(node => node.IsInGroup("Hurtboxes")))
        {
            hurtbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        }

        Action Clean = () =>
        {
            food.QueueFree();
            Velocity = TravelAxis * ActualSpeed;
            Rotation = TravelAxis.Angle();
        };

        Tween tweenScale = CreateTween();
        tweenScale.TweenProperty(food, "scale", Vector2.Zero, .5);
        tweenScale.TweenCallback(Callable.From(Clean));

    }

    private void Leave()
    {
        Velocity = TravelAxis * ActualSpeed;
        Rotation = TravelAxis.Angle();
    }
}

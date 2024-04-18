using Godot;
using Godot.Fish_the_fishes.Scripts;
using System.Collections.Generic;
using System.Linq;
using static Godot.TextServer;

public partial class SwordFish : Fish, IFisher
{
	private enum Action
	{
		Swimming,
		Seeking,
		Launched,
		Leaving
	}

	[Export]
	private int MaxStrikes = 5;
	[Export]
	private int MinStrikes = 1;

	public List<IFishable> FishedThings { get; } = new List<IFishable>();

	private int Strikes = 3;
	private CollisionShape2D HitBox;
	private Fish Target = null;
	private Action State = Action.Swimming;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		HitBox = GetNode<CollisionShape2D>("HitBox/CollisionShape2D");
		Strikes = GD.RandRange(MinStrikes, MaxStrikes);
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public override void GetCaughtBy(IFisher by)
	{
		base.GetCaughtBy(by);
		HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public override void Kill()
	{
		base.Kill();
		HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	private void SeekTarget()
	{
		if (!Actionable || State == Action.Seeking) return;

		Sprite.Animation = "seek";

		Node[] fishes = GetTree().GetNodesInGroup("Fishes")
			.Where(fish => (fish as Fish).IsAlive && (fish as Fish).IsOnScreen && !(fish is SwordFish))
			.ToArray();

		if (fishes.Length == 0)
		{
			Leave();
			return;
		}

		Target = (Fish)fishes[(int)(GD.Randi() % fishes.Length)];

		Velocity = Vector2.Zero;

		State = Action.Seeking;


		Tween tween = RotateAtConstantSpeed(GetDirectionTo(Target).Angle());
		tween.TweenCallback(Callable.From(() => CallDeferred("Launch")));

	}

	private void Launch()
	{
		if (!Actionable) return;
		if (!IsInstanceValid(Target))
		{
			State = Action.Swimming;
			SeekTarget();
			return;
		}

		Strikes--;

		Velocity = GetDirectionTo(Target);
		if (Velocity.Length() < ActualSpeed)
		{
			Velocity = Velocity.Normalized() * ActualSpeed;
		}
		Rotation = Velocity.Angle();

		Sprite.Animation = "dash";

		State = Action.Launched;
	}

	private void Leave()
	{
		Sprite.Animation = IsAlive ? "alive" : "dead";
		if (!Actionable) return;
		Velocity = new Vector2(ActualSpeed * (Flip ? -1 : 1), 0);

		RotateAtConstantSpeed(Velocity.Angle());

		State = Action.Leaving;
	}

	private void OnFishSkewered(Node2D body)
	{
		if (!(body is Fish) || FishedThings.Contains(body as Fish) || body == this) return;

		Fish Skew = body as Fish;

		Skew.Kill();
		Skew.GetCaughtBy(this);

		if (Skew == Target)
		{
			Velocity = Vector2.Zero;
			if (Strikes > 0)
			{
				SeekTarget();
			}
			else Leave();
		}
	}

	#region helpers
	private Vector2 GetDirectionTo(Fish target)
	{
		return Target.Position + (Target.Velocity * 0.8f) - Position;
	}

	private Tween RotateAtConstantSpeed(float angle)
	{
		Tween tween = CreateTween();
		angle = Mathf.LerpAngle(Rotation, angle, 1);
		// This timig allows for constant rotation velocity (1s for 180 degrees)
		float duration = Mathf.Abs(Rotation - angle) / Mathf.Pi;
		tween.TweenProperty(this, "rotation", angle, duration);

		return tween;
	}
	#endregion helpers
}

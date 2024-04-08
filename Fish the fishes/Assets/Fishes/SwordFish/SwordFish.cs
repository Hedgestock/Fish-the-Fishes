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

	//[Export]
	//private float DashSpeed = 600;

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        base._Process(delta);

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

		Node[] fishes = GetTree().GetNodesInGroup("Fishes")
            .Where(fish => (fish as Fish).IsAlive && (fish as Fish).IsOnScreen && !(fish is SwordFish))
            .ToArray();

        if (fishes.Length == 0) {
            GD.Print("No fishes found");
            Leave();
            return;
        }

		Target = (Fish) fishes[(int)(GD.Randi() % fishes.Length)];

        Velocity = Vector2.Zero;

        Tween tween = RotateAtConstantSpeed(GetDirectionTo(Target).Angle());
        tween.TweenCallback(Callable.From(() => CallDeferred("Launch")));

        State = Action.Seeking;
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
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

        Velocity = GetDirectionTo(Target);
        Rotation = Velocity.Angle();

        State = Action.Launched;
    }

    private void Leave()
    {
        if (!Actionable) return;
        Velocity = new Vector2(ActualSpeed, 0);

        RotateAtConstantSpeed(Velocity.Angle());

        State = Action.Leaving;
    }

	private void OnFishSkewered(Node2D body)
	{
        if (!(State == Action.Launched) || !(body is Fish) || body == this) return;

		Target = body as Fish;

		Target.Kill();
        Velocity = Vector2.Zero;
        Target.GetCaughtBy(this);

        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        if (Strikes > 0) SeekTarget();
        else Leave();
    }

    #region helpers
    private void AlterScore(Fish fish)
	{
		Value += fish.Value;
		if (fish.IsNegative) IsNegative = true;
		Multiplier *= fish.Multiplier;
    }

	private Vector2 GetDirectionTo(Fish target)
	{
		return Target.Position + (Target.Velocity * 0.8f) - Position;
    }

    private Tween RotateAtConstantSpeed(float angle)
    {
        Tween tween = CreateTween();
        angle = Mathf.LerpAngle(Rotation, angle, 1);
        // This timig allows for constant rotation velocity (1s for 180 degrees)
        tween.TweenProperty(this, "rotation", angle, Mathf.Abs(Rotation - angle) / Mathf.Pi);
        return tween;
    }
    #endregion helpers
}

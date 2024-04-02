using Godot;
using System.Linq;
using static Godot.TextServer;

public partial class SwordFish : Fish
{
    private enum Step
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

    private int Strikes = 3;
    private CollisionShape2D HitBox;
	private Fish Target = null;
	private Step Action = Step.Swimming;


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
    public override void Kill()
    {
        base.Kill();
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private void SeekTarget()
	{
        if (State != FishState.Alive || Action == Step.Seeking) return;

		Node[] fishes = GetTree().GetNodesInGroup("Fishes").Where(fish => (fish as Fish).State == FishState.Alive && !(fish is SwordFish)).ToArray();
		if (fishes.Length == 0) return;

		Target = (Fish) fishes[(int)(GD.Randi() % fishes.Length)];

        Velocity = Vector2.Zero;

        Tween tween = RotateAtConstantSpeed(GetDirectionTo(Target).Angle());
        tween.TweenCallback(Callable.From(() => CallDeferred("Launch")));

        Action = Step.Seeking;
        //VisibleOnScreenNotifier2D test = Target.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
    }

	private void Launch()
	{
        if (!IsInstanceValid(Target))
        {
            Action = Step.Swimming;
            SeekTarget();
            return;
        }

        Strikes--;
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

        Velocity = GetDirectionTo(Target);
        Rotation = Velocity.Angle();

        Action = Step.Launched;
    }

    private void Leave()
    {
        Velocity = new Vector2(ActualSpeed, 0);

        RotateAtConstantSpeed(Velocity.Angle());

        Action = Step.Leaving;
    }

	private void OnFishSkewered(Node2D body)
	{
        if (!(Action == Step.Launched) || !(body is Fish) || body == this) return;

		Target = body as Fish;

		Target.Kill();
        Velocity = Vector2.Zero;
        Target.Catch(Vector2.Zero);

		AlterScore(Target);


        Target.CallDeferred(Node.MethodName.Reparent, this);

        
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        if (Strikes > 0) SeekTarget();
        else Leave();
    }

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
}

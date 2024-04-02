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
    private int Strikes = 3;

    private CollisionShape2D HitBox;
	private Fish Target = null;
	private Step Action = Step.Swimming;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
		HitBox = GetNode<CollisionShape2D>("HitBox/CollisionShape2D");
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

		Action = Step.Seeking;
		
		Target = (Fish) fishes[(int)(GD.Randi() % fishes.Length)];
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);


        VisibleOnScreenNotifier2D test = Target.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

        Vector2 direction = GetDirectionTo(Target);
        Tween tween = GetTree().CreateTween();
        GD.Print(direction.Angle(), " , ", Rotation);
        tween.TweenProperty(this, "rotation", direction.Angle(), 1);
        tween.TweenCallback(Callable.From(Launch));
        Velocity = Vector2.Zero;
    }

	private void Launch()
	{
        Vector2 direction = GetDirectionTo(Target);
        Velocity = direction;
        Rotation = direction.Angle();
        Action = Step.Launched;
    }

    private void Leave()
    {
        Velocity = new Vector2(ActualSpeed, 0);

        Tween tween = GetTree().CreateTween();
        GD.Print(Velocity.Angle(), " , ", Rotation);
        tween.TweenProperty(this, "rotation", Velocity.Angle(), 1);

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
        Strikes--;
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
		return Target.Position + (Target.Velocity * 0.7f) - Position;
    }

    //private float GetClosestRotation(float angle)
    //{
    //    float res = angle;
    //    if (angle > 0)
    //}
}

using Godot;
using Godot.Collections;
using System;


public partial class FishingLine : RigidBody2D
{
	enum State
    {
		Stopped,
		Moving,
		Fishing,
		Hit,
		Resetting
    }

	[Signal]
	public delegate void ScoreEventHandler();

	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public uint speed;

	private Vector2 destination;
	private Vector2 start;
	private State state;

	private uint score;

	private Vector2 ScreenSize;
	private Vector2 BasePosition;

	private CollisionShape2D Hitbox;
	private AnimatedSprite2D Line;
	private Array<Fish> Fishes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
		BasePosition = new Vector2(ScreenSize.X / 2, 50);
		Position = BasePosition;
		state = State.Stopped;
		Hitbox = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
		Hitbox.Disabled = true;
		score = 0;
		Fishes = new Array<Fish>();
		Line = GetNode<AnimatedSprite2D>("Line");
		Line.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (state == State.Stopped) return;

		if (start.DistanceTo(Position) > start.DistanceTo(destination))
		{
			switch (state)
			{
				case State.Moving:
					state = State.Fishing;
					MoveTowards(new Vector2(ScreenSize.X / 2, -50));
					Hitbox.Disabled = false;
					Line.Animation = "weighted";
					break;
				case State.Fishing:
					Hitbox.Disabled = true;
					EmitSignal(SignalName.Score, score);
					score = 0;
					Fishes.Clear();
					Line.Animation = "loose";
					goto case State.Hit;
				case State.Hit:
					state = State.Resetting;
					MoveTowards(BasePosition);
					break;
				case State.Resetting:
					state = State.Stopped;
					LinearVelocity = new Vector2(0, 0);
					break;
				case State.Stopped:
				default:
					return;
			}
		}        
	}

    public override void _Input(InputEvent @event)
    {
		if (state != State.Stopped || Visible == false) return;
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton && @event.IsActionPressed("screen_tap"))
		{
			state = State.Moving;
			MoveTowards(eventMouseButton.Position);
		}
	}

	private void MoveTowards(Vector2 position)
    {
        destination = position;
        start = Position;
        LinearVelocity = (position - Position).Normalized() * speed;
    }

	void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Fish)
        {
			Fishes.Add(body as Fish);
			(body as Fish).LinearVelocity = LinearVelocity;
			score++;
        } else if (score > 0 && body is Trash)
        {
			EmitSignal(SignalName.Hit);
			score = 0;
			GetNode<AudioStreamPlayer>("HitSound").Play();
			foreach (Fish fish in Fishes)
            {
				fish.LinearVelocity = new Vector2(0, 0);
				fish.GravityScale = 1;
            }
			Fishes.Clear();
			LinearVelocity = new Vector2(0, 0);
			Line.Animation = "hit";
			Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            GetTree().CreateTimer(1).Timeout += () => { MoveTowards(destination); Line.Animation = "loose"; };
        }
	}
}

using Godot;
using Godot.Collections;
using System;


public partial class FishingLine : CharacterBody2D
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
	private bool invincible;

	private Vector2 screenSize;
	private Vector2 basePosition;

	private Area2D area;
	private CollisionShape2D hitbox;
	private AnimatedSprite2D line;
	private Array<Fish> fishes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screenSize = GetViewport().GetVisibleRect().Size;
		basePosition = new Vector2(screenSize.X / 2, 50);
		Position = basePosition;
		state = State.Stopped;
		invincible = false;
		area = GetNode<Area2D>("Area2D");
		hitbox = area.GetNode<CollisionShape2D>("CollisionShape2D");
		hitbox.Disabled = true;
		fishes = new Array<Fish>();
		line = GetNode<AnimatedSprite2D>("Line");
		line.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (state == State.Stopped) return;

        MoveAndSlide();


        if (start.DistanceTo(Position) > start.DistanceTo(destination))
		{
			switch (state)
			{
				case State.Moving:
					state = State.Fishing;
					MoveTowards(new Vector2(screenSize.X / 2, -50));
					hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
					line.Animation = "weighted";
					break;
				case State.Fishing:
					hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
					EmitSignal(SignalName.Score, ComputeScore());
					fishes.Clear();
					line.Animation = "loose";
					goto case State.Hit;
				case State.Hit:
					state = State.Resetting;
					MoveTowards(basePosition);
					break;
				case State.Resetting:
					state = State.Stopped;
					Velocity = new Vector2(0, 0);
					break;
				case State.Stopped:
				default:
					return;
			}
		} 
		
	}

	private void setInvicibility(bool invincibility)
	{
		invincible = invincibility;
	}

    private void MakeInvincible(Area2D area)
    {
		if (area == this.area)
		{
            setInvicibility(true);
		}
    }

    private void MakeVincible(Area2D area)
    {
        if (area == this.area)
        {
            setInvicibility(false);
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
        Velocity = (position - Position).Normalized() * speed;
	}

	void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Fish)
		{
			Fish fish = (Fish) body;
			fishes.Add(fish);
			fish.Catch(Velocity);
		} else if (fishes.Count > 0 && body is Trash && !invincible)
		{
            EmitSignal(SignalName.Hit);
			GetNode<AudioStreamPlayer>("HitSound").Play();
			foreach (Fish fish in fishes)
			{
				fish.Kill();
			}
			fishes.Clear();
			Velocity = new Vector2(0, 0);
			line.Animation = "hit";
			hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			GetTree().CreateTimer(1).Timeout += () => { MoveTowards(destination); line.Animation = "loose"; };
		}
	}

	private int ComputeScore()
	{
		float scoref = 0;
        foreach (Fish fish in fishes)
        {
			scoref += fish.Value;
        }
		int score = fibo2((int)Math.Ceiling(scoref));
        foreach (Fish fish in fishes)
        {
			if (fish.IsNegative)
			{
				score = -score;
				break;
			}
        }
        return score;
    }

	private int fibo2(int num)
	{
		if (num <= 1) return num;
		int prev = 1;
		int res = 1;
		for (int i = 0; i < num; i++)
		{
			int tmp = res;
			res += prev;
			prev = tmp;
		}
		return res - 1;
	}
}

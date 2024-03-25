using Godot;
using System;


public partial class Fish : RigidBody2D
{
	public bool flip = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LinearVelocity = new Vector2((float)GD.RandRange(150.0, 250.0) * (flip ? 1 : -1), 0);
        //GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = flip;
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }

    async private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		GetTree().CreateTimer(1).Timeout += QueueFree;
	}
}



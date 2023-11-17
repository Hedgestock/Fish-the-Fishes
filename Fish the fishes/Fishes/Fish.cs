using Godot;
using System;


public partial class Fish : RigidBody2D
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	async private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		GetTree().CreateTimer(1).Timeout +=() => { QueueFree(); };
	}
}



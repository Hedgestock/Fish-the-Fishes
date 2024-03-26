using Godot;
using System;

public partial class RedFish : Fish
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		if (flip)
		{
            if (flip)
            {
                Flip(GetNode<Area2D>("HitBox"));
            }
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_body_entered(Node body)
	{
        if (body != this && body is Fish)
		{
			(body as Fish).Kill();
		}
    }
}

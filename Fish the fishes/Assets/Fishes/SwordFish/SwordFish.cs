using Godot;
using Godot.Collections;
using System.Linq;

public partial class SwordFish : Fish
{
	[Export]
	public float dashSpeed = 600;

	private Fish target = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
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

	private void SeekTarget()
	{
		if (state != State.Alive) return;

		Node[] fishes = GetTree().GetNodesInGroup("Fishes").Where(fish => fish != this).ToArray();
		if (fishes.Length == 0) return;
		
		target = (Fish) fishes[(int)(GD.Randi() % fishes.Length)];
		GD.Print(target);
		Vector2 direction = target.Position - this.Position;
		Rotation = direction.Angle();
		LinearVelocity = direction;
		GD.Print(direction);
    }
}

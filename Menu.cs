using Godot;
using System;

public partial class Menu : CanvasLayer
{
	[Export]
	public PackedScene ClickIcon { get; set; }

	private Vector2 ScreenSize; // Size of the game window.

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Input(InputEvent @event)
	{
		// Mouse in viewport coordinates.
		if (@event is InputEventMouseButton eventMouseButton)
		{
			Sprite2D click = ClickIcon.Instantiate<Sprite2D>();
			click.Position = eventMouseButton.Position;
			AddChild(click);
		}
		

	}
	
	void _on_creep_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Dodge the creeps/Main.tscn");
	}

	private void _on_fishes_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Fish the fishes/FTFMain.tscn");
	}
}


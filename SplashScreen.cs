using Godot;
using System;

public partial class SplashScreen : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var modulator = GetNode<CanvasModulate>("CanvasModulate");
		modulator.Modulate = Colors.Black;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(GetNode<CanvasModulate>("CanvasModulate"), "modulate", Colors.White, 2.0f);
		tween.TweenProperty(GetNode<CanvasModulate>("CanvasModulate"), "modulate", Colors.White, 1.0f);
		tween.TweenProperty(GetNode<CanvasModulate>("CanvasModulate"), "modulate", Colors.Black, 2.0f);
		tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToFile("res://Fish the fishes/FTFMain.tscn")));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class Settings : CanvasLayer
{
	[Export]
	private CheckBox CompetitiveMode;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CompetitiveMode.ButtonPressed = UserSettings.CompetitiveMode;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GoToHome()
	{
		GetNode<GameManager>("/root/GameManager").ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

	private void SetCompetitiveMode(bool competition)
	{
		GetTree().Root.ContentScaleAspect = (competition ? Window.ContentScaleAspectEnum.Keep : Window.ContentScaleAspectEnum.Expand);
		UserSettings.CompetitiveMode = competition;
	}
}
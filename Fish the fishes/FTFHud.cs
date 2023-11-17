using Godot;
using System;

public partial class FTFHud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	[Signal]
	public delegate void EndGameEventHandler();

	private uint score = 0;
	private uint lives = 3;

	private HBoxContainer LivesContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LivesContainer = GetNode<HBoxContainer>("Lives");
		LivesContainer.GetNode<AnimatedSprite2D>("Life1").Play();
		LivesContainer.GetNode<AnimatedSprite2D>("Life2").Play();
		LivesContainer.GetNode<AnimatedSprite2D>("Life3").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_start_button_pressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<TextureButton>("BackButton").Hide();
		GetNode<Label>("Message").Hide();
		GetNode<Label>("Score").Show();
		LivesContainer.Show();
		EmitSignal(SignalName.StartGame);
	}
	private void _on_back_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}

	private void _on_fishing_line_score(uint score)
	{
		this.score += score;
		GetNode<Label>("Score").Text = this.score.ToString();
	}

	private void _on_fishing_line_hit()
	{
		lives--;
		LivesContainer.GetNode<AnimatedSprite2D>("Life" + (3 - lives)).Animation = "death";
		if (lives <= 0)
        {
			GetTree().CreateTimer(1).Timeout += () => { EmitSignal(SignalName.EndGame); ResetGame(); };
		}
	}

	private void ResetGame()
    {
		GetNode<Label>("Score").Hide();
		LivesContainer.Hide();
		GetNode<Button>("StartButton").Show();
		GetNode<TextureButton>("BackButton").Show();
		GetNode<Label>("Message").Text = "Score: \n" + score.ToString();
		GetNode<Label>("Message").Show();
		score = 0;
		lives = 3;
		LivesContainer.GetNode<AnimatedSprite2D>("Life1").Animation = LivesContainer.GetNode<AnimatedSprite2D>("Life2").Animation = LivesContainer.GetNode<AnimatedSprite2D>("Life3").Animation = "life";
	}
}


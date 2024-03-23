using Godot;

public partial class FTFHud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	[Signal]
	public delegate void EndGameEventHandler();

	private HBoxContainer LivesContainer;
	private GameManager gameManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameManager = GetNode<GameManager>("/root/GameManager");

        LivesContainer = GetNode<HBoxContainer>("Lives");
		LivesContainer.GetNode<AnimatedSprite2D>("Life1").Play();
		LivesContainer.GetNode<AnimatedSprite2D>("Life2").Play();
		LivesContainer.GetNode<AnimatedSprite2D>("Life3").Play();
        GetNode<Label>("MenuUI/HighScore").Text = "High Score: \n" + gameManager.highScore.ToString();

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	private void _on_start_button_pressed()
	{
		GetNode<Control>("MenuUI").Hide();
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
		gameManager.score += score;
		GetNode<Label>("Score").Text = gameManager.score.ToString();
	}

	private void _on_fishing_line_hit()
	{
        gameManager.lives--;
		LivesContainer.GetNode<AnimatedSprite2D>("Life" + (3 - gameManager.lives)).Animation = "death";
		if (gameManager.lives <= 0)
		{
			GetTree().CreateTimer(1).Timeout += () => { EmitSignal(SignalName.EndGame); ResetGame(); };
		}
	}

	private void ResetGame()
	{
		if (gameManager.score > gameManager.highScore) gameManager.highScore = gameManager.score;
		gameManager.SaveGame();
		GetNode<Label>("Score").Hide();
		LivesContainer.Hide();
		GetNode<Label>("MenuUI/Message").Text = "Last Score: \n" + gameManager.score.ToString();
		GetNode<Label>("MenuUI/HighScore").Text = "High Score: \n" + gameManager.highScore.ToString();
		GetNode<Control>("MenuUI").Show();
		gameManager.score = 0;
		gameManager.lives = 3;
		LivesContainer.GetNode<AnimatedSprite2D>("Life1").Animation = LivesContainer.GetNode<AnimatedSprite2D>("Life2").Animation = LivesContainer.GetNode<AnimatedSprite2D>("Life3").Animation = "life";
	}
}


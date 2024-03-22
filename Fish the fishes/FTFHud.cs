using Godot;

public partial class FTFHud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	[Signal]
	public delegate void EndGameEventHandler();

	private string saveFile = "user://data.save";
	private uint score = 0;
	private uint highScore = 0;
	private uint lives = 3;

	private HBoxContainer LivesContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LivesContainer = GetNode<HBoxContainer>("Lives");
		LivesContainer.GetNode<AnimatedSprite2D>("Life1").Play();
		LivesContainer.GetNode<AnimatedSprite2D>("Life2").Play();
		LivesContainer.GetNode<AnimatedSprite2D>("Life3").Play();
		LoadGame();
        GetNode<Label>("MenuUI/HighScore").Text = "High Score: \n" + highScore.ToString();

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
		if (score > highScore) highScore = score;
		SaveGame();
		GetNode<Label>("Score").Hide();
		LivesContainer.Hide();
		GetNode<Label>("MenuUI/Message").Text = "Last Score: \n" + score.ToString();
		GetNode<Label>("MenuUI/HighScore").Text = "High Score: \n" + highScore.ToString();
		GetNode<Control>("MenuUI").Show();
		score = 0;
		lives = 3;
		LivesContainer.GetNode<AnimatedSprite2D>("Life1").Animation = LivesContainer.GetNode<AnimatedSprite2D>("Life2").Animation = LivesContainer.GetNode<AnimatedSprite2D>("Life3").Animation = "life";
	}

	private Godot.Collections.Dictionary<string, Variant> Save()
	{
		return new Godot.Collections.Dictionary<string, Variant>()
		{
			{ "HighScore", highScore },
		};
	}

	private void SaveGame()
	{
		using var gameSave = FileAccess.Open(saveFile, FileAccess.ModeFlags.Write);

		gameSave.StoreLine(Json.Stringify(Save()));
	}

	private void LoadGame()
	{

        if (!FileAccess.FileExists(saveFile))
		{
			return;
		}

		using var saveGame = FileAccess.Open(saveFile, FileAccess.ModeFlags.Read);

		while (saveGame.GetPosition() < saveGame.GetLength())
		{
			var jsonString = saveGame.GetLine();

			// Creates the helper class to interact with JSON
			var json = new Json();
			var parseResult = json.Parse(jsonString);
			if (parseResult != Error.Ok)
			{
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
				continue;
			}

			// Get the data from the JSON object
			var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
			highScore = (uint) nodeData["HighScore"];
		}
	}
}


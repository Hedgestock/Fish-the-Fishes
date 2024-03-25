using Godot;
using System;

public partial class Home : CanvasLayer
{
    private GameManager gameManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        gameManager = GetNode<GameManager>("/root/GameManager");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

   	private void Play(Game.Mode mode)
	{
        gameManager.mode = mode;
        gameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Game/Game.tscn");
	}

    private void PlayClassic()
    {
        Play(Game.Mode.Classic);
    }
}

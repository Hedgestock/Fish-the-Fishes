using Godot;
using System;
using static Game;

public partial class PauseButton : TextureButton
{
    private void TogglePause()
    {
        GetTree().Paused = !GetTree().Paused;
    }

    private void QuitGame()
    {
        GetTree().Paused = false;
        if (GameManager.Mode > Mode.Training)
            SaveManager.SaveGame();
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }
}

using Godot;
using System;

public partial class PauseButton : TextureButton
{
    private void TogglePause()
    {
        GetTree().Paused = !GetTree().Paused;
    }

    private void QuitGame()
    {
        GetTree().Paused = false;
        SaveManager.SaveGame();
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }
}

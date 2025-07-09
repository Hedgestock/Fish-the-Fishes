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
        SaveManager.SaveData();
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }
}

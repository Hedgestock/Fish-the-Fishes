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
        SceneManager.ChangeSceneToFile("uid://blkg0i7vjb6il");
    }
}

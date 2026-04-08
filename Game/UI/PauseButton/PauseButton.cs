using Godot;
using System;
using static Game;
using static SceneManager;

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
        ChangeSceneToFile(HomeUID);
    }
}

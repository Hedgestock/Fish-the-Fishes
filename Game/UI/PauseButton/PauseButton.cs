using Godot;
using System;

public partial class PauseButton : TextureButton
{
    void OnPressed()
    {
        SaveManager.SaveGame();
        GetTree().Paused = !GetTree().Paused;
    }
}

using Godot;
using System;

public partial class PauseButton : TextureButton
{
    void OnPressed()
    {
        GetTree().Paused = !GetTree().Paused;
    }
}

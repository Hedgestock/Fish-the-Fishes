using Godot;
using System;

public partial class Credits : CanvasLayer
{
    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }
}

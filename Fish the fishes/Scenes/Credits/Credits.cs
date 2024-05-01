using Godot;
using System;

public partial class Credits : CanvasLayer
{
    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }
}

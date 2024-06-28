using Godot;
using System;

public partial class Equipment : CanvasLayer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }
}

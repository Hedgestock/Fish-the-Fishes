using Godot;
using System;

public partial class BackButton : TextureButton
{
    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }

    private void GoBack()
    {
        GameManager.GoToPreviousScene();
    }
}

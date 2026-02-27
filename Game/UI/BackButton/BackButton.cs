using Godot;
using System;

[GlobalClass]
public partial class BackButton : TextureButton
{
    private void GoToHome()
    {
        SceneManager.ChangeSceneToFile("uid://blkg0i7vjb6il");
    }

    private void GoBack()
    {
        SceneManager.GoToPreviousScene();
    }
}

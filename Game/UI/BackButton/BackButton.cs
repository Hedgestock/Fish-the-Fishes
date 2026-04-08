using Godot;
using System;
using static SceneManager;

[GlobalClass]
public partial class BackButton : TextureButton
{
    private void GoToHome()
    {
        ChangeSceneToFile(HomeUID);
    }

    private void GoBack()
    {
        SceneManager.GoToPreviousScene();
    }
}

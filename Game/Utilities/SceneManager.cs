using Godot;
using System.Collections.Generic;


public partial class SceneManager : Node
{
    protected static SceneManager _instance;
    public static SceneManager Instance { get { return _instance; } }

    public SceneManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    private static Stack<string> PrevSceneStack = new();

    public static string PrevScene
    {
        get
        {
            string prevScene = "";
            PrevSceneStack.TryPeek(out prevScene);
            return prevScene;
        }
    }

    // This method is here to give default value when connecting to a signal.
    static public void ChangeSceneToFile(string file)
    {
        ChangeSceneToFile(file, true);
    }
    static public void ChangeSceneToFile(string file, bool pushToQueue = true)
    {
        PrepareSceneChange(pushToQueue);
        _instance.GetTree().ChangeSceneToFile(file);
    }

    // This method is here to give default value when connecting to a signal.
    static public void ChangeSceneToPacked(PackedScene scene)
    {
        ChangeSceneToPacked(scene, true);
    }
    static public void ChangeSceneToPacked(PackedScene scene, bool pushToQueue = true)
    {
        PrepareSceneChange(pushToQueue);
        _instance.GetTree().ChangeSceneToPacked(scene);
    }

    static private void PrepareSceneChange(bool pushToQueue)
    {
        if (pushToQueue) PrevSceneStack.Push(_instance.GetTree().CurrentScene.SceneFilePath);
    }

    static public void GoToPreviousScene()
    {
        ChangeSceneToFile(PrevSceneStack.Pop(), false);
    }
}

using Godot;
using System.Collections.Generic;


public partial class SceneManager : Node
{
    public const string HomeUID = "uid://blkg0i7vjb6il";
    public const string GameUID = "uid://0sb3i5bm7j4p";
    public const string AquariumUID = "uid://bn4j1dckgh88u";
    public const string CompendiumUID = "uid://b0nv1b3x0suu5";
    public const string CreditsUID = "uid://cpc2lk8mjeebi";
    public const string EquipmentUID = "uid://jdvp2d8dcy53";
    public const string SettingsUID = "uid://dwv4cu7q6wlf6";
    public const string StatsUID = "uid://r1tsllld4k82";
    public const string TrainingUID = "uid://ereddneqlo34";
    public const string BiomeDebugUID = "uid://bv5ebnrlqpwku";
    public const string TutorialUID = "res://Game/Scenes/Tutorial/Tutorial.tscn";

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

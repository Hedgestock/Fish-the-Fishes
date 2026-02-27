using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using WaffleStock;

public partial class GameManager : Node
{
    [Signal]
    public delegate void TargetChangedEventHandler();

    [Signal]
    public delegate void BiomeChangedEventHandler();

    [Signal]
    public delegate void LifeUpEventHandler();

    [Export]
    private Array<WeightedBiome> _startingBiomes;
    [Export]
    private Biome _testBiome;

    public static Biome StartingBiome
    {
        get
        {
            string BiomeType = WeightedBiome.ChooseFrom(_instance._startingBiomes).ToString();
            return GD.Load<Biome>($"{Constants.BiomesFolder}{BiomeType}/{BiomeType}.tres");
        }
    }
    public static Biome TestBiome { get { return _instance._testBiome; } }

    private static Constants.Fishes _target = 0;
    static public Constants.Fishes Target { get { return _target; } }

    static public SaveManager.GameSave? GameSave = null;

    static public DateTime StartTime;

    private static Biome _biome;
    public static Biome Biome
    {
        get { return _biome; }
        set
        {
            _biome = value;

            _currentBiomeCatches = 0;
            CalculatedBiomeThreshold = (int)GD.Randfn(value.ChangeBiomeThreshold, value.ChangeBiomeThresholdDeviation);

            Instance.EmitSignal(SignalName.BiomeChanged);

            if (Mode <= Game.Mode.Training) return;
            if (UserData.BiomeCompendium.TryGetValue(value.ResourceName, out UserData.BiomeCompendiumEntry entry)) entry.Seen++;
            else UserData.BiomeCompendium[Biome.ResourceName] = new UserData.BiomeCompendiumEntry();
        }
    }

    public static int Flip = 0;

    public static Game.Mode Mode = Game.Mode.Menu;
    public static long Score = 0;
    public static uint Lives = 3;
    private static int _currentBiomeCatches = 0;
    public static int CurrentBiomeCatches
    {
        get { return _currentBiomeCatches; }
        set
        {
            if (value > CalculatedBiomeThreshold)
            {
                ChangeBiome();
            }
            else
            {
                _currentBiomeCatches = value;
            }
        }
    }
    public static int CalculatedBiomeThreshold = 0;

    public static Vector2 ScreenSize;

    static private GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }

    private GameManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewport().GetVisibleRect().Size;
        GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(OnScreenResize));
    }

    static public void ChangeTarget()
    {
        _target = WeightedFish.ChooseFrom(Biome.Fishes);
        _instance.EmitSignal(SignalName.TargetChanged);
    }

    static public void ChangeBiome()
    {
        if (Biome.FollowupBiomes.Count == 0) return;
        Biome = GD.Load<Biome>(Biome.GetRandomPathFrom(Biome.FollowupBiomes));

        // That's a mouthfull, but we simply check the current biome to check if the target is still valid
        // otherwise, we just set a new one.
        if (Mode == Game.Mode.Target && !Biome.Fishes.Select(fish => fish.Item).Contains(Target))
            ChangeTarget();
    }

    private void OnScreenResize()
    {
        ScreenSize = GetViewport().GetVisibleRect().Size;
    }
}

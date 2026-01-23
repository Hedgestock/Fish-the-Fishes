using Godot;
using System;
using System.Collections.Generic;
using WaffleStock;
using static WaffleStock.Constants;

public partial class SpawnHandler : Node
{
    [Export]
    Game Game;

    [Export]
    OptionButton LoadedFish;

    public bool Enabled = false;

    Vector2 SpawnPosition;
    Vector2 Diff;

    public override void _Ready()
    {
        base._Ready();
        Fishes[] fishes = Enum.GetValues<Fishes>();
        foreach (Fishes fish in fishes)
        {
            LoadedFish.AddItem(fish.ToString(), (int)fish);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Mouse in viewport coordinates.
        if (!Enabled || !(@event is InputEventMouseButton eventMouseButton)) return;

        if (@event.IsActionPressed("screen_tap"))
        {
            SpawnPosition = eventMouseButton.Position;
        }
        else if (@event.IsActionReleased("screen_tap") && (Diff = eventMouseButton.Position - SpawnPosition).Length() > 50)
        {
            string FishType = Enum.GetName<Constants.Fishes>((Constants.Fishes)LoadedFish.Selected);
            
            Game.SpawnFish($"{Constants.FishesFolder}{FishType}/{FishType}.tscn", SpawnPosition, Diff.Normalized());
            GetViewport().SetInputAsHandled();
        }
    }

    void SetEnabled(bool enabled)
    {
        Enabled = enabled;
        LoadedFish.Visible = enabled;
    }
}

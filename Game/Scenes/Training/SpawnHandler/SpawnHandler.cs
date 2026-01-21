using Godot;
using System;

public partial class SpawnHandler : Node
{
    [Export]
    Game Game;

    public bool Enabled = true;

    Vector2 SpawnPosition;
    Vector2 Diff;
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
            Game.SpawnFish(SpawnPosition, Diff.Normalized());
            GetViewport().SetInputAsHandled();
        }
    }

    void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }
}

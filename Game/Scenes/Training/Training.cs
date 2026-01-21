using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Training : CanvasLayer
{
    [Export]
    Game Game;

    Callable ConnectColorBoxes;

    public override void _Ready()
    {
        base._Ready();

        ConnectColorBoxes = Callable.From<Node>((node) =>
        {
            Callable.From<Node2D>(FindAndConnect).CallDeferred(node);
        });
    }

    void VisibleCollisions(bool visible)
    {
        if (Visible)
        {
            Game.Connect(SignalName.ChildEnteredTree, ConnectColorBoxes);
            FindAndConnect(Game);
        }
        else
        {
            Game.Disconnect(SignalName.ChildEnteredTree, ConnectColorBoxes);
        }
    }

    void FindAndConnect(Node node)
    {
        if (node is CollisionShape2D cShape)
            cShape.Connect(Node2D.SignalName.Draw, Callable.From(() => DrawCollision(cShape)));
        foreach (var child in node.GetChildren())
            FindAndConnect(child);
    }

    void DrawCollision(CollisionShape2D shape)
    {
        shape.Shape.Draw(shape.GetCanvasItem(), shape.DebugColor);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Visible == false) return;
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton && @event.IsActionPressed("screen_hold"))
        {
            Game.SpawnFish(eventMouseButton.Position);
        }
    }
}
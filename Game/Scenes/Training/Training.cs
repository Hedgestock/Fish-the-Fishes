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
            if (!(node is Node2D node2d)) return;
            Callable.From<Node2D>(FindAndConnect).CallDeferred(node);
        });
    }

    void VisibleCollisions(bool visible)
    {
        if (Visible)
        {
            Game.Connect(SignalName.ChildEnteredTree, ConnectColorBoxes);
            ConnectColorBoxes.Call(Game);
        }
        else
        {
            Game.Disconnect(SignalName.ChildEnteredTree, ConnectColorBoxes);
        }
    }

    void FindAndConnect(Node2D node)
    {
        foreach (var child in node.GetChildren().OfType<Node2D>())
        {
            if (child is CollisionShape2D cShape)
                cShape.Connect(Node2D.SignalName.Draw, Callable.From(() => DrawCollision(cShape)));
            FindAndConnect(child);
        }
    }

    void DrawCollision(CollisionShape2D shape)
    {
        shape.Shape.Draw(shape.GetCanvasItem(), shape.DebugColor);
    }
}
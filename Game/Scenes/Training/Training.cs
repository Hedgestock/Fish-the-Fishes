using Godot;
using System;
using System.Linq;

public partial class Training : CanvasLayer
{
    [Export]
    Game Game;

    public override void _Ready()
    {
        Game.Connect(SignalName.ChildEnteredTree, Callable.From<Node>(ConnectColorBoxes));
        base._Ready();
    }

    void ConnectColorBoxes(Node node)
    {
        if (!(node is Node2D node2d)) return;
        node.Connect(Node2D.SignalName.Draw, Callable.From(() => ColorBoxes(node2d)));
        ColorBoxes(node2d);
    }

    void ColorBoxes(Node2D node)
    {
        //GetTree().CreateTimer(.1).Timeout +=
        Callable.From(() =>
        {
            foreach (var child in node.GetChildren().OfType<Node2D>())
            {
                if (child is CollisionShape2D cShape)
                    cShape.Shape.Draw(cShape.GetCanvasItem(), cShape.DebugColor);
                ColorBoxes(child);
            }
        }).CallDeferred();
    }
}
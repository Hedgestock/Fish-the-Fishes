using Godot;

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
        if (visible)
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
        Color drawColor = shape.DebugColor;

        if (shape.Disabled)
        {
            drawColor = new Color(drawColor.V, drawColor.V, drawColor.V, 0.5f);
        }

        shape.Shape.Draw(shape.GetCanvasItem(), drawColor);
    }
}
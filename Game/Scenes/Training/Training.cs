using Godot;

public partial class Training : CanvasLayer
{
    [Export]
    Game Game;

    Callable SetRendering;

    public override void _Ready()
    {
        base._Ready();

        SetRendering = Callable.From<Node>((node) => FindAndSetRendering(node, true));
    }

    void VisibleCollisions(bool visible)
    {
        if (visible)
        {
            Game.Connect(SignalName.ChildEnteredTree, SetRendering, (uint)ConnectFlags.Deferred);
            FindAndSetRendering(Game, true);
        }
        else
        {
            Game.Disconnect(SignalName.ChildEnteredTree, SetRendering);
            FindAndSetRendering(Game, false);
        }
    }

    void FindAndSetRendering(Node node, bool rendering)
    {
        if (node is CollisionShape2D cShape)
            cShape.Rendering = rendering;
        foreach (var child in node.GetChildren())
            FindAndSetRendering(child, rendering);
    }
}
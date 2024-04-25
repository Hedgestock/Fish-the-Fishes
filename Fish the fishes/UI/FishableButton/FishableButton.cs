using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class FishableButton : StaticBody2D, IFishable
{
    private CollisionShape2D CollisionShape;

    public bool IsCaught { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public IFishable GetCaughtBy(IFisher by)
    {
        CallDeferred(Node.MethodName.Reparent, by as Node);
        return this;
    }

    public void OnPressed()
    {
        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }
}

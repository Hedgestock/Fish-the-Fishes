using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class FishableButton : StaticBody2D, IFishable
{
    private CollisionShape2D CollisionShape;
    private Button Button;

    public bool IsCaught { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        Button = GetNode<Button>("Button");
        CollisionShape.Shape = new RectangleShape2D();
        ((RectangleShape2D)CollisionShape.Shape).Size = Button.Size;
        CollisionShape.Position = Button.Size / 2;
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

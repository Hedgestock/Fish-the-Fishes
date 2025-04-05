using Godot;
using WaffleStock;
using System;

public partial class FishableButton : StaticBody2D, IFishable
{
    CollisionShape2D CollisionShape;
    Button Button;
    Vector2 BasePosition;

    bool _isCaught = false;
    public bool IsCaught { get => _isCaught; set => _isCaught = value; }
    public bool IsInDisplay { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        Button = GetNode<Button>("Button");
        CollisionShape.Shape = new RectangleShape2D();
        ((RectangleShape2D)CollisionShape.Shape).Size = Button.Size;
        CollisionShape.Position = Button.Size / 2;
        BasePosition = Position;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!IsCaught)
            Position = new Vector2(BasePosition.X + (float)Math.Sin((new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds + Position.X * 3) / (200 * Math.PI)) * 5, BasePosition.Y + (float)Math.Sin((new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds + Position.Y * 3) / (200 * Math.PI)) * 5); ;
    }

    public IFishable GetCaughtBy(IFisher by)
    {
        IsCaught = true;
        CallDeferred(Node.MethodName.Reparent, by as Node);
        return this;
    }

    public void OnPressed()
    {
        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }
}

using Godot;
using WaffleStock;
using System;

public partial class FishableButton : StaticBody2D, IFishable
{
    CollisionShape2D CollisionShape;
    Button Button;
    Vector2 BasePosition;

    public bool IsCaught { get; set; }
    public bool CantGetCaught { get; set; }

    public bool IsInDisplay { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        IsCaught = false;
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

    public bool Escape(IFisher fisher)
    {
        return false;
    }

    public void GetCaughtBy(IFisher fisher)
    {
        IsCaught = true;
        CallDeferred(Node.MethodName.Reparent, fisher as Node);
    }

    public void OnPressed()
    {
        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }
}

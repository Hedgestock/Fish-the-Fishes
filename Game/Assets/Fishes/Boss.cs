using Godot;
using System;

public partial class Boss : Fish
{
    [Export]
    private int MaxPasses = 5;
    [Export]
    private int MinPasses = 3;

    [Export]
    private int MaxWait = 12;
    [Export]
    private int MinWait = 3;

    protected int Passes;

    public override void _Ready()
    {
        base._Ready();

        Passes = GD.RandRange(3, 5);
    }

    protected virtual void PreparePass()
    {
        SetScale();

        SetPosition();

        SetTravelAxis();

        Velocity = TravelAxis * ActualSpeed;
        Rotation = (float)(TravelAxis.Angle() - (Flip ? Mathf.Pi : 0));
    }

    protected override void Despawn()
    {
        if (IsAlive && Passes > 0)
        {
            Passes--;
            var parent = GetParent();
            Flip = !Flip;
            GetTree().CreateTimer(GD.RandRange(MinWait, MaxWait)).Timeout += () =>
            {
                parent.AddChild(this);
                PreparePass();
            };
            parent.RemoveChild(this);
        }
        else
        {
            base.Despawn();
        }
    }
}

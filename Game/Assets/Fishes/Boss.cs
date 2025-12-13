using Godot;
using System;

public partial class Boss : Fish
{
    [Signal]
    public delegate void DespawningEventHandler();

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

        PreparePass();
    }

    protected virtual void PreparePass() { }

    protected override void Despawn()
    {
        if (Passes > 0)
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
            EmitSignalDespawning();
            base.Despawn();
        }
    }
}

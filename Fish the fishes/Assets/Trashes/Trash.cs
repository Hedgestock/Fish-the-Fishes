using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class Trash : CharacterBody2D, IFishable
{
    [Export]
    private float GravityScale = 0.2f;

    private GameManager GM;

    public bool IsCaught { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        velocity.Y += (float)delta * GravityScale * (int)ProjectSettings.GetSetting("physics/2d/default_gravity");
        Velocity = velocity;
        MoveAndSlide();
    }

    public IFishable GetCaughtBy(IFisher by)
    {
        if (by.GetType() == typeof(FishingLine))
        {
            if (GM.Mode == Game.Mode.GoGreen)
            {
                if (by.FishedThings.Contains(this))
                    return this; // This avoids multiple calls on reparenting
                IsCaught = true;
                by.FishedThings.Add(this);
                CallDeferred(Node.MethodName.Reparent, by as Node);
                Velocity = Vector2.Zero;
                GravityScale = 0;
            }
            else
            {
                (by as FishingLine).GetHit(FishingLine.DamageType.Trash);
            }
        }
        return this;
    }
    protected void Despawn()
    {
        if (!IsCaught)
        {
            QueueFree();
        }
    }
}



using Godot;
using System;
using WaffleStock;

public partial class BossGiantSquidEye : StaticBody2D, IFishable
{
    [Signal]
    public delegate void ClosingEventHandler();

    public bool IsInDisplay { get; set; }
    public bool IsCaught {
        get
        {
            return Sprite.Animation == "closed";
        }
        set
        {
            Sprite.Animation = value ? "closed" : "open";
            if (value) EmitSignalClosing();
        }
    }

    [Export]
    AnimatedSprite2D Sprite;

    public IFishable GetCaughtBy(IFisher by)
    {
        IsCaught = true;
        return this;
    }
}

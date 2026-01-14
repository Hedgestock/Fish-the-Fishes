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
            if (value && Sprite.Animation != "closed") EmitSignalClosing();
            Sprite.Animation = value ? "closed" : "open";
        }
    }

    public bool CantGetCaught { get; set; }

    [Export]
    AnimatedSprite2D Sprite;

    public bool Escape(IFisher fisher)
    {
        return false;
    }

    public void GetCaughtBy(IFisher fisher)
    {
        IsCaught = true;
    }
}

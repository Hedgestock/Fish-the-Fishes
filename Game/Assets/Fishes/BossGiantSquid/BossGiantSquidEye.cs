using Godot;
using System;
using WaffleStock;

public partial class BossGiantSquidEye : StaticBody2D, IFishable
{
    public bool IsInDisplay { get; set; }
    public bool IsCaught {
        get
        {
            return Sprite.Animation == "closed";
        }
        set
        {
            Sprite.Animation = value ? "closed" : "open";
        }
    }

    [Export]
    AnimatedSprite2D Sprite;

    public IFishable GetCaughtBy(IFisher by)
    {
        IsCaught = true;
        GD.Print("Catching squid eye");
        return this;
    }
}

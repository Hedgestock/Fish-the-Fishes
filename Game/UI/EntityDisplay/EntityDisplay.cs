using Godot;
using System;
using WaffleStock;

public partial class EntityDisplay : Container
{
    public CharacterBody2D Entity
    {
        set
        {
            var vosn = value.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
            EntityViewport.Size = (Vector2I)(vosn.Rect.Size * vosn.Scale);
            SetEntityDiplaysSize();
            value.Position = (vosn.Rect.Size * vosn.Scale / 2) - vosn.Position;
            ((IFishable)value).IsInDisplay = true;
            EntityViewport.AddChild(value);
        }
    }

    [Export]
    ScrollContainer Scroll;
    [Export]
    SubViewport EntityViewport;

    [Export]
    Container ZoomButtons;
    [Export]
    Container AnimationButtons;

    public override void _Ready()
    {
        base._Ready();
        GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(OnScreenResize));
    }

    private void SetEntityDiplaysSize()
    {
        CustomMinimumSize = Scroll.CustomMinimumSize = new(Math.Min(GetViewportRect().Size.X / 2, EntityViewport.Size.X), Math.Min(1000, EntityViewport.Size.Y));
    }

    private void OnScreenResize()
    {
        SetEntityDiplaysSize();
    }

}

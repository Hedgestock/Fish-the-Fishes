using Godot;
using System;
using WaffleStock;

public partial class EntityDisplay : SubViewportContainer
{
    public CharacterBody2D Entity
    {
        set
        {
            var tmp = value.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
            Viewport.Size = (Vector2I)(tmp.Rect.Size * tmp.Scale);
            value.Position = tmp.Rect.Size * tmp.Scale / 2;
            ((IFishable)value).IsInDisplay = true;
            Viewport.AddChild(value);
        }
    }

    [Export]
    SubViewport Viewport;
}

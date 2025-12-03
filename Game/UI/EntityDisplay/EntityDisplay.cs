using Godot;
using System;
using WaffleStock;

public partial class EntityDisplay : ScrollContainer
{
    public CharacterBody2D Entity
    {
        set
        {
            var tmp = value.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
            Viewport.Size = (Vector2I)(tmp.Rect.Size * tmp.Scale);
            CustomMinimumSize = new(Math.Min(GetViewportRect().Size.X / 2, Viewport.Size.X), Math.Min(1000, Viewport.Size.Y));
            value.Position = tmp.Rect.Size * tmp.Scale / 2;
            ((IFishable)value).IsInDisplay = true;
            Viewport.AddChild(value);
        }
    }

    [Export]
    SubViewport Viewport;
}

using Godot;
using System;

public partial class EntityDisplay : SubViewportContainer
{
    Node2D Entity
    {
        set
        {
            value.ProcessMode = ProcessModeEnum.Disabled;
            Viewport.AddChild(value);
        }
    }

    [Export]
    SubViewport Viewport;
}

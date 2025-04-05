using Godot;
using System;

public partial class EntityDisplay : SubViewportContainer
{
    public CharacterBody2D Entity
    {
        set
        {
            value.Position = Vector2.One * 100;
            value.Rotation = 0;
            value.SetPhysicsProcess(false);
            Viewport.AddChild(value);
        }
    }

    [Export]
    SubViewport Viewport;
}

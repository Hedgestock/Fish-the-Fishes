using Godot;
using System;

public partial class Blood : GpuParticles2D
{
    public override void _Ready()
    {
        base._Ready();
        Emitting = true;
        GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
    }
}

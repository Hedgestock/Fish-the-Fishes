using Godot;
using System;

public partial class Bubbles : GpuParticles2D
{
    public void DelayedDespawn()
    {
        Emitting = false;
        GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
    }
}

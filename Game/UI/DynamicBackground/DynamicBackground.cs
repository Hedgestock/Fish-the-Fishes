using Godot;
using System;

public partial class DynamicBackground : TextureRect
{
    public override void _Ready()
    {
        base._Ready();
        Texture = GameManager.Biome?.Background ?? Texture;
    }
}

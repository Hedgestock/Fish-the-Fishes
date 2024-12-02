using Godot;
using System;

public partial class Hook : Node
{

    public Area2D Area;
    public CollisionShape2D Hitbox;

    public override void _Ready()
    {
        Area = GetNode<Area2D>("Area2D");
        Hitbox = Area.GetNode<CollisionShape2D>("CollisionShape2D");
    }
}

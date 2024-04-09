using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class FishableButton : StaticBody2D, IFishable
{
	[Export]
	PackedScene TargetScene;

    private GameManager GM;
    private CollisionShape2D CollisionShape;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GM = GetNode<GameManager>("/root/GameManager");
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void GetCaughtBy(IFisher by)
    {
        CallDeferred(Node.MethodName.Reparent, by as Node);
    }

    public void OnPressed()
    {
        CollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }

    public void ChangeScene()
	{
        GM.ChangeSceneToPacked(TargetScene);
	}
}

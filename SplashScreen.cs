using Godot;
using Godot.Collections;
using System;

public partial class SplashScreen : CanvasLayer
{
	[Export]
	Array<ParticleProcessMaterial> Materials { get; set; }

	public override void _Ready()
	{
		foreach (var material in Materials)
		{
			var instance = new GpuParticles2D();
			instance.ProcessMaterial = material;
			instance.OneShot = true;
			instance.Emitting = true;
			AddChild(instance);
		}
		var background = GetNode<ColorRect>("ColorRect");
		background.Modulate = Colors.Black;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(background, "modulate", Colors.White, 2.0f);
		tween.TweenProperty(background, "modulate", Colors.White, 1.0f);
		tween.TweenProperty(background, "modulate", Colors.Black, 2.0f);
		tween.TweenCallback(Callable.From(() => GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn")));

        AchievementsManager.UnlockEquipment("StandardHook", EquipmentPiece.Type.Hook);
    }
}

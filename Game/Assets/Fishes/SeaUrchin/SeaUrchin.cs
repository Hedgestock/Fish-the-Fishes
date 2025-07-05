using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using WaffleStock;

public partial class SeaUrchin : Fish, IFisher
{
    [Export]
    public Array<Constants.Fishes> ImmuneToSkew = new Array<Constants.Fishes>();

    [Export]
    CollisionShape2D HitBox;

    List<Line2D> spikes;

    int growth = -1;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    public override void _Ready()
    {
        spikes = Sprite.GetChildren().OfType<Line2D>().ToList();

        if (!IsInDisplay)
        {
            float positionOffset = VisibleOnScreenNotifier.Rect.Size.Y * VisibleOnScreenNotifier.Scale.Y / 2;
            Position = new Vector2(
                    (float)GD.RandRange(positionOffset, GameManager.ScreenSize.X - positionOffset),
                    (float)GD.RandRange(GameManager.ScreenSize.Y * SpawnRange.X, GameManager.ScreenSize.Y * SpawnRange.Y)
                );
        }

        base._Ready();

        if (IsInDisplay) return;

        Scale = Vector2.Zero;

        Tween tween = CreateTween();
        tween.TweenProperty(this, "scale", Vector2.One * ActualSizeVariation, 0.3);
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        growth = 0;
        Grow();

        Tween tweenGrowth = CreateTween();
        tweenGrowth.TweenProperty(HitBox.Shape, "radius", 174, 0.25).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

        return base.GetCaughtBy(by);
    }

    //public override void Kill()
    //{
    //    base.Kill();
    //    HitBox.Disabled = true;
    //}

    private void Grow()
    {
        if (growth >= 10) return;
        foreach (Line2D spike in spikes)
        {
            Vector2[] tmp = spike.Points.Append(spike.Points[spike.Points.Length - 1]).ToArray();
            spike.Points = tmp;
        }

        Tween tweenGrowth = CreateTween();
        tweenGrowth.TweenMethod(
            Callable.From<int>((p) =>
            {
                foreach (Line2D spike in spikes)
                {
                    Vector2[] tmp = spike.Points;
                    tmp[tmp.Length - 1].X = p;
                    spike.Points = tmp;
                }
            }),
            -48 - (growth * 10),
            -48 - (++growth * 10),
            0.03);
        tweenGrowth.TweenCallback(Callable.From(Grow));
    }

    private void OnFishSkewered(Node2D body)
    {
        //TODO: Check that we're not fishing a parent
        if (!(body is Fish) || FishedThings.Contains(body as Fish) || body == this || CheckImmunity(ImmuneToSkew, body.GetType())) return;

        Fish Skew = body as Fish;

        Skew = Skew.GetCaughtBy(this) as Fish;
        Skew.Kill();
    }
}

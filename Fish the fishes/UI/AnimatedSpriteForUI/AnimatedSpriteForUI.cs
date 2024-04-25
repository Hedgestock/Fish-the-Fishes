using Godot;
using System;

public partial class AnimatedSpriteForUI : BoxContainer
{
    [Export]
    AnimatedSprite2D Sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetTree().Root.SizeChanged += AdaptPosition;
    }

    private void TakeSpace()
    {
        CustomMinimumSize = Sprite.SpriteFrames.GetFrameTexture(Sprite.Animation, 0).GetSize();
        GD.Print(CustomMinimumSize);
    }

    private void AdaptPosition()
    {
        Sprite.GlobalPosition = new Vector2(GlobalPosition.X + Size.X / 2, GlobalPosition.Y + Size.Y / 2);
        GD.Print(Sprite.GlobalPosition);
    }

    #region AnimatedSprite2D Forwarding

    #region Properties
    public StringName Animation
    {
        get { return Sprite.Animation; }
        set { Sprite.Animation = value; }
    }

    public string AutoPlay
    {
        get { return Sprite.Autoplay; }
        set { Sprite.Autoplay = value; }
    }

    public bool Centered
    {
        get { return Sprite.Centered; }
        set { Sprite.Centered = value; }
    }

    public bool FlipH
    {
        get { return Sprite.FlipH; }
        set { Sprite.FlipH = value; }
    }

    public bool FlipV
    {
        get { return Sprite.FlipV; }
        set { Sprite.FlipV = value; }
    }

    public int Frame
    {
        get { return Sprite.Frame; }
        set { Sprite.Frame = value; }
    }

    public float FrameProgress
    {
        get { return Sprite.FrameProgress; }
        set { Sprite.FrameProgress = value; }
    }

    public Vector2 Offset
    {
        get { return Sprite.Offset; }
        set { Sprite.Offset = value; }
    }

    public float SpeedScale
    {
        get { return Sprite.SpeedScale; }
        set { Sprite.SpeedScale = value; }
    }

    public SpriteFrames SpriteFrames
    {
        get { return Sprite.SpriteFrames; }
        set
        {
            Sprite.SpriteFrames = value;
            TakeSpace();
            CallDeferred(MethodName.AdaptPosition);
        }
    }
    #endregion

    #region Methods
    public float GetPlayingSpeed()
    {
        return Sprite.GetPlayingSpeed();
    }

    public bool IsPlaying()
    {
        return Sprite.IsPlaying();
    }

    public void Pause()
    {
        Sprite.Pause();
    }

    public void Play(StringName name = null, float customSpeed = 1f, bool fromEnd = false)
    {
        Sprite.Play(name, customSpeed, fromEnd);
    }

    public void PlayBackwards(StringName name = null)
    {
        Sprite.PlayBackwards(name);
    }

    public void SetFrameAndProgress(int frame, float progress)
    {
        Sprite.SetFrameAndProgress(frame, progress);
    }

    public void Stop()
    {
        Sprite.Stop();
    }
    #endregion

    #endregion
}

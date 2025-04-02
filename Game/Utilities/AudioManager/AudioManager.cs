using Godot;
using System;

public partial class AudioManager : Node
{
    [Export]
    private AudioStreamPlayer UISounds;
    [Export]
    private AudioStreamPlayer Ambiance;
    [Export]
    private AudioStreamPlayer Music;

    static private AudioManager _instance = null;
    public static AudioManager Instance { get { return _instance; } }

    private AudioManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void _Ready()
    {
        base._Ready();

    }

    public static void UIPlay(AudioStream sound)
    {
        _instance.UISounds.Stream = sound;
        _instance.UISounds.Play();
    }

    public static void PlayMusic(AudioStream music)
    {
        if (music == _instance.Music.Stream) return;
        Tween tween = _instance.CreateTween();
        if (_instance.Music.Stream != null)
            tween.TweenProperty(_instance.Music, "volume_linear", 0, .5f);
        tween.TweenCallback(
            Callable.From(() =>
            {
                _instance.Music.Stream = music;
                _instance.Music.Play();
            }));
        tween.TweenProperty(_instance.Music, "volume_linear", 1, .5f);
    }

    public static void StopMusic()
    {
        Tween tween = _instance.CreateTween();
        tween.TweenProperty(_instance.Music, "volume_linear", 0, .5f);
        tween.TweenCallback(
            Callable.From(() =>
            {
                _instance.Music.Stop();
                _instance.Music.Stream = null;
            }));
    }
}

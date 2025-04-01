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
        _instance.Music.Stream = music;
        _instance.Music.Play();
    }

    public static void StopMusic()
    {
        _instance.Music.Stop();
    }
}

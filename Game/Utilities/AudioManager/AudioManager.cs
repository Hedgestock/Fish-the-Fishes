using Godot;
using Godot.Collections;
using System.Linq;

public partial class AudioManager : Node
{

    [Export]
    private Array<AudioStreamPlayer> Players;

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

    private bool _paused;

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_paused != GetTree().Paused)
        {
            _paused = GetTree().Paused;
            foreach (var player in Players)
            {
                SetPlayerVolume(player, _paused ? .5f : 1);
            }
        }
    }

    public static void UIPlay(AudioStream sound)
    {
        var UIPlayer = _instance.Players.First(player => player.Bus == "SFX");
        UIPlayer.Stream = sound;
        UIPlayer.Play();
    }

    public static void PlayMusic(AudioStream music)
    {
        var musicPlayer = _instance.Players.First(player => player.Bus == "Music");
        if (music == musicPlayer.Stream) return;
        Tween tween = _instance.CreateTween();
        if (musicPlayer.Stream != null)
            tween.TweenProperty(musicPlayer, "volume_linear", 0, .5f);
        tween.TweenCallback(
            Callable.From(() =>
            {
                musicPlayer.Stream = music;
                musicPlayer.Play();
            }));
        tween.TweenProperty(musicPlayer, "volume_linear", 1, .5f);
    }

    public static void StopMusic()
    {
        var musicPlayer = _instance.Players.First(player => player.Bus == "Music");
        Tween tween = _instance.CreateTween();
        tween.TweenProperty(musicPlayer, "volume_linear", 0, .5f);
        tween.TweenCallback(
            Callable.From(() =>
            {
                musicPlayer.Stop();
                musicPlayer.Stream = null;
            }));
    }

    public static void SetPlayerVolume(AudioStreamPlayer player, float volume)
    {
        Tween tween = _instance.CreateTween();
        tween.TweenProperty(player, "volume_linear", volume, .5f);
    }
}

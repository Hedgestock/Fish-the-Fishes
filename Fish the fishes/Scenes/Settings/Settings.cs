using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class Settings : CanvasLayer
{
    [Export]
    private Container DeleteDataPopup;

    [Export]
    private CheckBox CompetitiveMode;

    [Export]
    private CheckBox MuteMaster;
    [Export]
    private Slider MasterVolume;

    [Export]
    private CheckBox MuteMusic;
    [Export]
    private Slider MusicVolume;

    [Export]
    private CheckBox MuteSFX;
    [Export]
    private Slider SFXVolume;

    [Export]
    private CheckBox MuteFishes;
    [Export]
    private Slider FishesVolume;

    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");
        CompetitiveMode.ButtonPressed = UserSettings.CompetitiveMode;

        MuteMaster.ButtonPressed = UserSettings.MuteMaster;
        MuteMusic.ButtonPressed = UserSettings.MuteMusic;
        MuteSFX.ButtonPressed = UserSettings.MuteSFX;
        MuteFishes.ButtonPressed = UserSettings.MuteFishes;

        MasterVolume.Value = UserSettings.MasterVolume;
        MusicVolume.Value = UserSettings.MusicVolume;
        SFXVolume.Value = UserSettings.SFXVolume;
        FishesVolume.Value = UserSettings.FishesVolume;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void GoToHome()
    {
        GetNode<GameManager>("/root/GameManager").ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private void SetCompetitiveMode(bool competition)
    {
        GetTree().Root.ContentScaleAspect = (competition ? Window.ContentScaleAspectEnum.Keep : Window.ContentScaleAspectEnum.Expand);
        UserSettings.CompetitiveMode = competition;
    }

    private void MuteMasterVolume(bool mute)
    {
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), mute);
        UserSettings.MuteMaster = mute;
    }
    private void MasterVolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(volume));
        UserSettings.MasterVolume = volume;
    }

    private void MuteMusicVolume(bool mute)
    {
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), mute);
        UserSettings.MuteMusic = mute;
    }
    private void MusicVolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(volume));
        UserSettings.MusicVolume = volume;
    }

    private void MuteSFXVolume(bool mute)
    {
        AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), mute);
        UserSettings.MuteSFX = mute;
    }
    private void SFXVolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), Mathf.LinearToDb(volume));
        UserSettings.SFXVolume = volume;
    }

    private void MuteFishesVolume(bool mute)
    {
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Fishes"), mute);
        UserSettings.MuteFishes = mute;
    }
    private void FishesVolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Fishes"), Mathf.LinearToDb(volume));
        UserSettings.FishesVolume = volume;
    }

    private void DisplayDeleteDataPopup()
    {
        DeleteDataPopup.Show();
    }

    private void HideDeleteDataPopup()
    {
        DeleteDataPopup.Hide();
    }

    private void DeleteData()
    {
        GameManager.EraseData();
        HideDeleteDataPopup();
    }
}

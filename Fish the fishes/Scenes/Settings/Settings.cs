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


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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

    private void GoToHome()
    {
        GameManager.SaveSettings();
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private void SetCompetitiveMode(bool competition)
    {
        UserSettings.CompetitiveMode = competition;
    }

    private void MuteMasterVolume(bool mute)
    {
        UserSettings.MuteMaster = mute;
    }
    private void MasterVolumeChanged(float volume)
    {
        UserSettings.MasterVolume = volume;
    }

    private void MuteMusicVolume(bool mute)
    {
        UserSettings.MuteMusic = mute;
    }
    private void MusicVolumeChanged(float volume)
    {
        UserSettings.MusicVolume = volume;
    }

    private void MuteSFXVolume(bool mute)
    {
        UserSettings.MuteSFX = mute;
    }
    private void SFXVolumeChanged(float volume)
    {
        UserSettings.SFXVolume = volume;
    }

    private void MuteFishesVolume(bool mute)
    {
        UserSettings.MuteFishes = mute;
    }
    private void FishesVolumeChanged(float volume)
    {
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

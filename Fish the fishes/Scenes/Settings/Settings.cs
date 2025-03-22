using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class Settings : CanvasLayer
{
    [Export]
    private Container DeleteDataPopup;

    [Export]
    private CustomCheckBox CompetitiveMode;

    [Export]
    private CustomCheckBox MuteMaster;
    [Export]
    private Slider MasterVolume;

    [Export]
    private CustomCheckBox MuteMusic;
    [Export]
    private Slider MusicVolume;

    [Export]
    private CustomCheckBox MuteSFX;
    [Export]
    private Slider SFXVolume;

    [Export]
    private CustomCheckBox MuteAmbiance;
    [Export]
    private Slider AmbianceVolume;

    [Export]
    private CustomCheckBox MuteFishes;
    [Export]
    private Slider FishesVolume;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CompetitiveMode.ButtonPressed = UserSettings.CompetitiveMode;

        MuteMaster.ButtonPressed = !UserSettings.MuteMaster;
        MuteMusic.ButtonPressed = !UserSettings.MuteMusic;
        MuteSFX.ButtonPressed = !UserSettings.MuteSFX;
        MuteAmbiance.ButtonPressed = !UserSettings.MuteAmbiance;
        MuteFishes.ButtonPressed = !UserSettings.MuteFishes;

        MasterVolume.Value = UserSettings.MasterVolume;
        MusicVolume.Value = UserSettings.MusicVolume;
        SFXVolume.Value = UserSettings.SFXVolume;
        AmbianceVolume.Value = UserSettings.AmbianceVolume;
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

    private void MuteMasterVolume(bool on)
    {
        UserSettings.MuteMaster = !on;
    }
    private void MasterVolumeChanged(float volume)
    {
        UserSettings.MasterVolume = volume;
    }

    private void MuteMusicVolume(bool on)
    {
        UserSettings.MuteMusic = !on;
    }
    private void MusicVolumeChanged(float volume)
    {
        UserSettings.MusicVolume = volume;
    }

    private void MuteSFXVolume(bool on)
    {
        UserSettings.MuteSFX = !on;
    }
    private void SFXVolumeChanged(float volume)
    {
        UserSettings.SFXVolume = volume;
    }

    private void MuteAmbianceVolume(bool on)
    {
        UserSettings.MuteAmbiance = !on;
    }
    private void AmbianceVolumeChanged(float volume)
    {
        UserSettings.AmbianceVolume = volume;
    }

    private void MuteFishesVolume(bool on)
    {
        UserSettings.MuteFishes = !on;
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

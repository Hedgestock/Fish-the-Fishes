using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class VolumeControl : VBoxContainer
{
    [Export]
    Button MuteCheckBox;
    [Export]
    Slider VolumeSlider;

    enum AudioBus { }
    [Export]
    AudioBus Bus;

    public override void _ValidateProperty(Dictionary property)
    {
        base._ValidateProperty(property);
        if (property["name"].AsStringName() == PropertyName.Bus)
        {
            int busNumber = AudioServer.BusCount;
            property["hint_string"] = "";
            for (int i = 0; i < busNumber; i++)
            {
                if (i > 0)
                    property["hint_string"] += ",";
                var busName = AudioServer.GetBusName(i);
                property["hint_string"] += busName;
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        MuteCheckBox.ButtonPressed = !AudioServer.IsBusMute((int)Bus);
        MuteCheckBox.Text = AudioServer.GetBusName((int)Bus) + " Volume";
        VolumeSlider.Value = AudioServer.GetBusVolumeLinear((int)Bus);
    }

    void MuteVolume(bool on)
    {
        AudioServer.SetBusMute((int)Bus, !on);
    }
    void VolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeLinear((int)Bus, volume);
    }
}

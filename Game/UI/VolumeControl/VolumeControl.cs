using Godot;
using Godot.Collections;

[Tool, GlobalClass, Icon("./VolumeControl.svg")]
public partial class VolumeControl : VBoxContainer
{
    CheckBox MuteCheckBox;
    HSlider VolumeSlider;

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
        MuteCheckBox = new()
        {
            ButtonPressed = !AudioServer.IsBusMute((int)Bus),
            Text = AudioServer.GetBusName((int)Bus) + " Volume",
            Flat = true
        };
        MuteCheckBox.Connect(CheckBox.SignalName.Toggled, Callable.From<bool>(MuteVolume));
        AddChild(MuteCheckBox);

        GD.Print(AudioServer.GetBusName((int)Bus), AudioServer.GetBusVolumeLinear((int)Bus));

        VolumeSlider = new()
        {
            MaxValue = 1,
            Step = 0.01f,
            Value = AudioServer.GetBusVolumeLinear((int)Bus),
        };
        VolumeSlider.Connect(HSlider.SignalName.ValueChanged, Callable.From<float>(VolumeChanged));
        AddChild(VolumeSlider);
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

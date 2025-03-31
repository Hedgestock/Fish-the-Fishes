using Godot;
using System;

public partial class CustomCheckBox : Button
{
    [Export]
    private Texture2D Checked;

    [Export]
    private Texture2D Unchecked;

    [Export]
    private Texture2D CheckMark;

    public override void _Ready()
    {
        base._Ready();
        Icon = Unchecked;
    }

    private void OnToggle(bool toggled)
    {
        Icon = toggled ? Checked : Unchecked;
    }
}

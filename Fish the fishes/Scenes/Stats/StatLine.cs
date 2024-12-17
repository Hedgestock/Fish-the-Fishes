using Godot;
using System;

public partial class StatLine : HBoxContainer
{
    [Export]
    public Label ScoreLabel { get; set; }

    public uint Score { set { ScoreLabel.Text = value.ToString(); } }
}

using Godot;
using System;

public partial class StatLine : HBoxContainer
{
    [Export]
    private Label ScoreLabel { get; set; }

    public uint Score { set { ScoreLabel.Text = value.ToString(); } }
}

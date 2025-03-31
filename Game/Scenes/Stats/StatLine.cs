using Godot;
using System;

public partial class StatLine : HBoxContainer
{
    [Export]
    public Label ScoreLabel { get; set; }

    public long Score { set { ScoreLabel.Text = value.ToString(); } }
}

using Godot;
using System;

public partial class Credits : CanvasLayer
{
    [Export]
    TabContainer Tabs;

    public override void _Ready()
    {
        base._Ready();
        Tabs.SetTabTitle(0, Tr("CREDITS"));
        Tabs.SetTabTitle(1, Tr("PATCH_NOTES"));
    }
}

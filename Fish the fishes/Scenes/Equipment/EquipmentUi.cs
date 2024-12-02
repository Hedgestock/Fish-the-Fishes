using Godot;
using System;

public partial class EquipmentUi : PanelContainer
{
    [Export]
    public AnimatedSpriteForUI Placeholder { get; set; }

    public override void _Input(InputEvent @event)
    {
        GD.Print("LOL",  @event);
    }
}

using Godot;
using System;

public partial class ArrowFish : Fish
{
    public override void _Ready()
    {
        base._Ready();
        if (Flip) GameManager.Flip++;
        else GameManager.Flip--;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationPredelete)
        {
            if (!Flip) GameManager.Flip++;
            else GameManager.Flip--;
        }
        base._Notification(what);
    }
}

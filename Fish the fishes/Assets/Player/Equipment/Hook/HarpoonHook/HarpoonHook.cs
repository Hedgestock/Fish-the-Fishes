

using static Godot.WebSocketPeer;

public partial class HarpoonHook : Hook
{
    public override Action State
    {
        get { return _state; }
        set
        {
            _state = value;
            DisableHitbox(_state != Action.MovingDown);
        }
    }
}

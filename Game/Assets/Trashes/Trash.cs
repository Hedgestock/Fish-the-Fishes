using Godot;
using Godot.FishTheFishes;
using System;

public partial class Trash : CharacterBody2D, IFishable, IDescriptible
{
    [Export]
    private float GravityScale = 0.2f;

    [ExportGroup("Compendium")]
    [Export]
    public string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string CompendiumDescription { get; set; }

    public bool IsCaught { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NotifySpawn();
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        velocity.Y += (float)delta * GravityScale * (int)ProjectSettings.GetSetting("physics/2d/default_gravity");
        Velocity = velocity;
        MoveAndSlide();
    }

    public IFishable GetCaughtBy(IFisher by)
    {
        if (GameManager.Mode == Game.Mode.GoGreen && by is FishingLine)
        {
            if (by.FishedThings.Contains(this))
                return this; // This avoids multiple calls on reparenting
            IsCaught = true;
            by.FishedThings.Add(this);
            CallDeferred(Node.MethodName.Reparent, by as Node);
            Velocity = Vector2.Zero;
            GravityScale = 0;
        }
        return this;
    }

    protected void Despawn()
    {
        if (!IsCaught)
        {
            QueueFree();
        }
    }

    protected void NotifySpawn()
    {
        if (GameManager.Mode == Game.Mode.Menu) return;
        if (UserData.TrashCompendium.TryGetValue(GetType().Name, out UserData.TrashCompendiumEntry entry)) entry.Seen++;
        else UserData.TrashCompendium[GetType().Name] = new UserData.TrashCompendiumEntry();
    }
}



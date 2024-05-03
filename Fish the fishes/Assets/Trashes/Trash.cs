using Godot;
using Godot.Fish_the_fishes.Scripts;
using Godot.Fish_the_fishes.Scripts.Interfaces;
using System;

public partial class Trash : CharacterBody2D, IFishable, IDescriptible
{
    public static string CompendiumName { get { return "Trash"; } }
    public static string CompendiumDescription { get { return "This is a trash"; } }

    [Export]
    private float GravityScale = 0.2f;

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
        if (by is FishingLine)
        {
            if (GameManager.Mode == Game.Mode.GoGreen)
            {
                if (by.FishedThings.Contains(this))
                    return this; // This avoids multiple calls on reparenting
                IsCaught = true;
                by.FishedThings.Add(this);
                CallDeferred(Node.MethodName.Reparent, by as Node);
                Velocity = Vector2.Zero;
                GravityScale = 0;
            }
            else
            {
                if (by.FishedThings.Count == 0 || (by as FishingLine).IsInvincible) return this;

                UserData.TrashCompendium[GetType().Name].Hit++;

                (by as FishingLine).GetHit(FishingLine.DamageType.Trash);
            }
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
        if (UserData.TrashCompendium.TryGetValue(GetType().Name, out UserData.TrashCompendiumEntry entry))
        {
            entry.Seen++;
        }
        else
        {
            UserData.TrashCompendium[GetType().Name] = new UserData.TrashCompendiumEntry();
        }
    }
}



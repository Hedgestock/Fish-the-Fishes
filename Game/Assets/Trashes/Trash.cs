using Godot;
using WaffleStock;

public partial class Trash : CharacterBody2D, IDescriptible
{
    [Export]
    private float GravityScale = 0.2f;

    [ExportGroup("Compendium")]
    [Export]
    public string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string CompendiumDescription { get; set; }

    public bool IsInDisplay { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NotifySpawn();
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsInDisplay) return;
        var velocity = Velocity;
        velocity.Y += (float)delta * GravityScale * (int)ProjectSettings.GetSetting("physics/2d/default_gravity");
        Velocity = velocity;
        MoveAndSlide();
    }

    protected void Despawn()
    {
        if (!IsInDisplay)
                   QueueFree();
            }

    protected void NotifySpawn()
    {
        if (GameManager.Mode <= Game.Mode.Training) return;
        if (UserData.TrashCompendium.TryGetValue(GetType().Name, out UserData.TrashCompendiumEntry entry)) entry.Seen++;
        else UserData.TrashCompendium[GetType().Name] = new UserData.TrashCompendiumEntry();
    }
}



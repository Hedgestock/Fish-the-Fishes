using Godot;
using WaffleStock;

[GlobalClass]
public partial class Achievement : Resource, IAchievable
{
    public virtual IAchievable.CheckTiming Timing { get; }
    [Export]
    public virtual string UnlockableName { get; set; }
    [Export]
    public virtual EquipmentPiece.Type UnlockableType { get; set; }
    [Export]
    public virtual string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public virtual string CompendiumDescription { get; set; }

    public virtual bool Predicate() { return false; }
}

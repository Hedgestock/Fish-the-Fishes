using Godot;
using WaffleStock;

[GlobalClass]
public partial class WeightedTrash : WeightedItem<Constants.Trashes>
{
    [Export]
    new public Constants.Trashes Item
    {
        get { return base.Item; }
        set { base.Item = value; }
    }
}

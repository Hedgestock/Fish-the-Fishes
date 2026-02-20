using Godot;
using WaffleStock;

public partial class SpawnLine<ItemType> : HBoxContainer
{
    [Export]
    public CheckBox CheckBox;

    WeightedItem<ItemType> _item;
    public WeightedItem<ItemType> Item
    {
        get { return _item; }
        set
        {
            _item = value;
            CheckBox.Text = value.Item.ToString();
        }
    }

    private void OnWeightChanged(float weight)
    {
        _item.Weight = (uint)weight;
    }

    private void OnSpawnToggle(bool spawning)
    {
        switch (Item)
        {
            case WeightedFish weightedFish:
                if (spawning)
                    GameManager.Biome.Fishes.Add(weightedFish);
                else
                    GD.Print(GameManager.Biome.Fishes.Remove(weightedFish));
                break;
            case WeightedTrash weightedTrash:
                if (spawning)
                    GameManager.Biome.Trashes.Add(weightedTrash);
                else
                    GD.Print(GameManager.Biome.Trashes.Remove(weightedTrash));
                break;
        }
    }
}

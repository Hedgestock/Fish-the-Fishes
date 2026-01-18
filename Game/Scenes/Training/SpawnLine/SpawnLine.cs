using Godot;
using System;

public partial class SpawnLine : HBoxContainer
{
    [Export]
    public CustomCheckBox CheckBox;

    WeightedFish _item;
    public WeightedFish Item
    {
        get { return _item; }
        set
        {
            _item = value;
            CheckBox.Text = _item.Fish.ToString();
        }
    }

    private void OnWeightChanged(float weight)
    {
        _item.Weight = (uint)weight;
        GD.Print($"Changed {_item.Fish.ToString()} weight to {weight}");
    }


    private void OnSpawnToggle(bool spawning)
    {
        switch (Item)
        {
            case WeightedFish weightedFish:
                if (spawning)
                {
                    GameManager.Biome.Fishes.Add(weightedFish);
                    GD.Print(weightedFish.Weight);
                }
                else
                    GD.Print(GameManager.Biome.Fishes.Remove(weightedFish));
                break;
        }
    }
}

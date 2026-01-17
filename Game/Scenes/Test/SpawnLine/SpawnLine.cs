using Godot;
using System;

public partial class SpawnLine : HBoxContainer
{
    [Export]
    CustomCheckBox CheckBox;

    WeightedFish _item;
    public WeightedFish Item
    {
        get { return _item; }
        set {
            _item = value;
            CheckBox.Text = _item.Fish.ToString();
        }
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
        }
    }
}

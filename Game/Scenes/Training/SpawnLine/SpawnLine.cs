using Godot;
using System;

public partial class SpawnLine : HBoxContainer
{
    [Export]
    public CheckBox CheckBox;

    WeightedItem _item;
    public WeightedItem Item
    {
        get { return _item; }
        set
        {
            _item = value;
            switch (value)
            {
                case WeightedFish wf:
                    CheckBox.Text = wf.Fish.ToString();
                    break;
                case WeightedTrash wt:
                    CheckBox.Text = wt.Trash.ToString();
                    break;
            }
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

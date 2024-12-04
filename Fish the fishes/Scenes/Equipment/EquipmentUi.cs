using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;

public partial class EquipmentUi : TextureButton
{
    private string Key { get; set; }
    private EquipmentPiece.Type Type { get; set; }
    private Action EquipStuff { get; set; }

    public void SetUI(string key, EquipmentPiece.Type type, SpriteFrames spriteFrames, Action equipStuff)
    {
        Key = key;
        Type = type;
        TextureNormal = spriteFrames.GetFrameTexture("default", 0);
        EquipStuff = equipStuff;
    }

    private void OnClick()
    {
        if (UserData.Equipments[Key].IsEquipped) return;
        foreach (var equipment in UserData.Equipments.Values.Where(e => e.Type == Type))
        {
            equipment.IsEquipped = false;
        }
        UserData.Equipments[Key].IsEquipped = true;
        EquipStuff?.Invoke();

    }
}

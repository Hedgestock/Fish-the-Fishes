using Godot;
using Wafflestock;
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
        if (!UserData.Equipments.ContainsKey(key))
        {
            Disabled = true;
            Modulate = new Color("Black");
        }
        EquipStuff = equipStuff;
    }

    private void OnClick()
    {
        // We're not doing that anymore to recover from potential multiequiped stuff
        // (which shouldn't happen in theory)
        //if (UserData.Equipments[Key].IsEquipped) return;
        foreach (var equipment in UserData.Equipments.Values.Where(e => e.Type == Type))
        {
            equipment.IsEquipped = false;
        }
        UserData.Equipments[Key].IsEquipped = true;
        EquipStuff?.Invoke();
    }
}

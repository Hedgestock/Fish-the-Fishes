using Godot;
using WaffleStock;
using System;
using System.Linq;

public partial class EquipmentUi : TextureButton
{
    private string Key { get; set; }
    private EquipmentPiece.Type Type { get; set; }
    private Action EquipStuff { get; set; }

    public void SetUI(string key, EquipmentPiece.Type type, SpriteFrames spriteFrames, Action displayInfo)
    {
        Key = key;
        Type = type;
        TextureNormal = spriteFrames.GetFrameTexture("default", 0);
        if (!UserData.Equipments.ContainsKey(key))
        {
            Disabled = true;
            FocusMode = FocusModeEnum.None;
            Modulate = new Color("Black");
        }
        Connect(SignalName.Pressed, Callable.From(displayInfo));
    }



    private void Focus(bool grabbed)
    {
        if (grabbed)
            Material = GD.Load<ShaderMaterial>("uid://bcg4hkkxm8epo");
        else
            Material = null;

        // We're not doing that anymore to recover from potential multiequiped stuff
        // (which shouldn't happen in theory)
        //if (UserData.Equipments[Key].IsEquipped) return;
        //foreach (var equipment in UserData.Equipments.Values.Where(e => e.Type == Type))
        //{
        //    equipment.IsEquipped = false;
        //}
        //UserData.Equipments[Key].IsEquipped = true;
        //EquipStuff?.Invoke();
    }
}

using Godot;
using WaffleStock;
using System;
using System.Linq;

public partial class Equipment : CanvasLayer
{
    [Export]
    private Node GameContainer;

    [Export]
    private FishingLine FishingLine;

    [Export]
    private Control PlaceHolder;
    [Export]
    private Container DescriptionContainer;
    [Export]
    private Label EquipmentName;
    [Export]
    private Label EquipmentDescription;
    [Export]
    private Button EquipButton;

    [Export]
    private Container Menu;

    [Export]
    private PackedScene EquipmentUITemplate;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Populate(EquipmentPiece.Type.Hook, FishingLine.EquipHook);
        Populate(EquipmentPiece.Type.Line, FishingLine.EquipLine);
        Populate(EquipmentPiece.Type.Weight, FishingLine.EquipWeigth);
        Populate(EquipmentPiece.Type.Attractor, FishingLine.EquipAttractor);
    }

    private void Populate(EquipmentPiece.Type pieceType, Action equip)
    {
        HFlowContainer container = new();
        Menu.AddChild(container);
        foreach (var piece in Constants.EquipmentList[pieceType])
        {
            var button = EquipmentUITemplate.Instantiate<EquipmentUi>();
            var equipmentPiece = GD.Load<PackedScene>(piece.Value).Instantiate<EquipmentPiece>();
            button.SetUI(piece.Key, pieceType, equipmentPiece.SpriteFrames, () => DisplayEquipment(equipmentPiece, piece.Key, equip));
            container.AddChild(button);
        }
    }

    private void DisplayEquipment(EquipmentPiece equipmentPiece, string equipmentKey, Action equip)
    {
        EquipmentName.Text = equipmentPiece.EquipmentName;
        EquipmentDescription.Text = equipmentPiece.EquipmentDescription;
        PlaceHolder.Visible = false;
        DescriptionContainer.Visible = true;

        var connections = EquipButton.GetSignalConnectionList(Button.SignalName.Pressed);

        foreach (var connection in connections)
        {
            Callable callable = (Callable)connection["callable"];
            EquipButton.Disconnect(Button.SignalName.Pressed, callable);
        }

        if (UserData.Equipments.ContainsKey(equipmentKey) && UserData.Equipments[equipmentKey].IsEquipped)
        {
            if (equipmentPiece.EquipmentType == EquipmentPiece.Type.Line || equipmentPiece.EquipmentType == EquipmentPiece.Type.Hook)
            {
                EquipButton.Visible = false;
            }
            else
            {
                EquipButton.Visible = true;
                EquipButton.Text = "Unequip";
                EquipButton.Connect(Button.SignalName.Pressed, Callable.From(() => Equip(null, equipmentKey, equip)));
            }
        }
        else
        {
            EquipButton.Visible = true;
            EquipButton.Text = "Equip";
            EquipButton.Connect(Button.SignalName.Pressed, Callable.From(() => Equip(equipmentPiece, equipmentKey, equip)));
        }
    }

    private void Equip(EquipmentPiece equipmentPiece, string equipmentKey, Action equip)
    {
        HideEquipment();
        if (equipmentPiece == null)
            UserData.Equipments[equipmentKey].IsEquipped = false;
        else
        {
            foreach (var equipmentStatus in UserData.Equipments.Where(e => e.Value.Type == equipmentPiece.EquipmentType && e.Value.IsEquipped))
                equipmentStatus.Value.IsEquipped = false;
            UserData.Equipments[equipmentKey] = new UserData.EquipmentStatus(equipmentPiece.EquipmentType, true);
        }
        equip();
    }

    private void HideEquipment()
    {
        PlaceHolder.Visible = true;
        DescriptionContainer.Visible = false;
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }

    private void SpawnFish()
    {
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        GameContainer.AddChild(fish);
    }
}

using Godot;
using WaffleStock;

public partial class Settings : CanvasLayer
{
    [Export]
    Container DeleteDataPopup;

    [Export]
    CustomCheckBox CompetitiveMode;
    [Export]
    CustomCheckBox Vibrations;
    [Export]
    CustomCheckBox WaterEffect;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print(UserSettings.CompetitiveMode, UserSettings.Vibrations, UserSettings.WaterEffect);
        GD.Print(CompetitiveMode.ButtonPressed, Vibrations.ButtonPressed, WaterEffect.ButtonPressed);
        CompetitiveMode.ButtonPressed = UserSettings.CompetitiveMode;
        Vibrations.ButtonPressed = UserSettings.Vibrations;
        WaterEffect.ButtonPressed = UserSettings.WaterEffect;
        GD.Print(CompetitiveMode.ButtonPressed, Vibrations.ButtonPressed, WaterEffect.ButtonPressed);
    }

    private void GoToHome()
    {
        SaveManager.SaveSettings();
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }

    private void SetCompetitiveMode(bool competition)
    {
        UserSettings.CompetitiveMode = competition;
    }

    private void SetVibrations(bool vibrations)
    {
        UserSettings.Vibrations = vibrations;
    }

    private void SetWaterEffect(bool effect)
    {
        UserSettings.WaterEffect = effect;
    }

    private void DisplayDeleteDataPopup()
    {
        DeleteDataPopup.Show();
    }

    private void HideDeleteDataPopup()
    {
        DeleteDataPopup.Hide();
    }

    private void DeleteData()
    {
        SaveManager.EraseData();
        HideDeleteDataPopup();
    }
}

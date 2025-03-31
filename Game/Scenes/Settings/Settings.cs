using Godot;
using Wafflestock;

public partial class Settings : CanvasLayer
{
    [Export]
    Container DeleteDataPopup;

    [Export]
    CustomCheckBox CompetitiveMode;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CompetitiveMode.ButtonPressed = UserSettings.CompetitiveMode;
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

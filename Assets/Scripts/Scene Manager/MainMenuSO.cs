using UnityEngine;

public enum MenuType
{
    MainMenu,
    PauseMenu
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
public class MainMenuSO : GameScene
{
    [Header("Menu Specific")]
    public MenuType type;
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum MenuType
{
    MainMenu,
    PauseMenu
}

[System.Serializable]
public struct Gamescore
{
    public string name;
    public int totalLine;
    public string time;
}

public class GameScene : ScriptableObject
{
    [Header("Information")]
    public string sceneName;
    public string shortDesc;

    [Header("Sounds")]
    public AudioClip music;

    [Header("Visual")]
    public VolumeProfile volumeProfile;
}

[CreateAssetMenu(fileName = "NewLevel", menuName = "Scene Data/Level")]
public class Level : GameScene
{
    [Header("Thumbnail")]
    public Sprite levelThumbnail;
    [Header("PlayerScore")]
    public Gamescore[] scores;
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
public class Menu : GameScene
{
    [Header("Menu Specific")]
    public MenuType type;
}

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public List<Menu> menus = new List<Menu>();
    public int CurrentLevelIndex = 1;

    public void LoadLevelWithIndex(int index)
    {
        if (index <= levels.Count)
        {
            SceneManager.LoadSceneAsync("Level_" + index.ToString());
        }
        else CurrentLevelIndex = 1;
    }

    public void LoadNextLevel()
    {
        CurrentLevelIndex++;
        LoadLevelWithIndex(CurrentLevelIndex);
    }

    public void RestartLevel()
    {
        LoadLevelWithIndex(CurrentLevelIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)MenuType.MainMenu].sceneName);
    }

    public void LoadPauseMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)MenuType.PauseMenu].sceneName);
    }
}
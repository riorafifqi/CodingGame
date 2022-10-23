using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public List<MainMenuSO> menus = new List<MainMenuSO>();
    public int CurrentLevelIndex = 0;

    public void LoadLevelWithIndex(int index)
    {
        if (index <= levels.Count)
        {
            SceneManager.LoadSceneAsync("Level_" + index.ToString());
        }
        else CurrentLevelIndex = 0;
    }

    public void LoadLevelWithLevel(Level lvl)
    {
        Debug.Log(lvl.sceneName);
        SceneManager.LoadSceneAsync(lvl.sceneName);
    }

    public void LoadNextLevel()
    {
        UnlockNextLevel();
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

    public void UnlockNextLevel()
    {
        CurrentLevelIndex++;
        levels[CurrentLevelIndex].status = Status.unlocked;
    }
}

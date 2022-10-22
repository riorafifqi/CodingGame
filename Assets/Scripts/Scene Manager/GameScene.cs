using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public List<MainMenuSO> menus = new List<MainMenuSO>();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanelManager : MonoBehaviour
{
    public TMP_Text lineCount;
    public TMP_Text time;
    
    public TMP_Text status;

    public ScenesData scenesData;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameObject.SetActive(false);
    }

    public void OpenWinPanel()
    {
        gameObject.SetActive(true);
        scenesData.UnlockNextLevel();
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.coding));
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.coding));
    }

    public void MenuButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.exit));
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SetLineCount(int count)
    {
        lineCount.text = count.ToString();
    }

    public void SetTime(float count)
    {
        time.text = count.ToString() + "s";
    }

    public void SetStatus(string sentence)
    {
        status.text = sentence;
    }
}

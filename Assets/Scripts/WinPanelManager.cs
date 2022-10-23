using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanelManager : MonoBehaviour
{
    public TMP_Text lineCount;
    public TMP_Text time;

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
    }

    public void ContinueButton()
    {
        scenesData.LoadNextLevel();

    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}

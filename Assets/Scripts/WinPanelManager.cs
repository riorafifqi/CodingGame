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
    [SerializeField] GameObject succeedPanel;        // For Single player
    [SerializeField] GameObject winPanel;       // For Multiplayer
    [SerializeField] GameObject losePanel;      // For Multiplayer
    [SerializeField] GameObject drawPanel;      // For Multiplayer

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //gameObject.SetActive(false);
    }

    public void OpenSucceedPanel()
    {
        succeedPanel.SetActive(true);
        scenesData.UnlockNextLevel();
    }

    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void OpenLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void OpenDrawPanel()
    {
        drawPanel.SetActive(true);
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
        SceneManager.LoadSceneAsync("RevampedMainMenu");
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

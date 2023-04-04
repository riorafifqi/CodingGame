using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    bool isPaused = false;


    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            SoundManager.Instance.PauseMusic();
            SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.pause));
            isPaused = true;
            pausePanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            Unpause();
        }
    }

    public void Unpause()
    {
        SoundManager.Instance.ResumeMusic();
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.resume));
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void SettingButton()
    {
        PlayOpenMenuSound();
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        settingsPanel.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    void PlayOpenMenuSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.open_menu));
    }

    public void BackToMainMenu()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.exit));
        SceneManager.LoadSceneAsync("RevampedMainMenu");
    }
}

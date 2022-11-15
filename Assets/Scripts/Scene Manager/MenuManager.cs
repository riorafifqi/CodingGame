using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;

    public void PlayButton()
    {
        PlayOpenMenuSound();
        levelSelectPanel.SetActive(!levelSelectPanel.activeSelf);
        levelSelectPanel.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void PlayMultiplayerButton()
    {
        SceneManager.LoadScene("MultiplayerLoadingScreen");
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

    public void ExitGame()
    {
        Application.Quit();
    }
}

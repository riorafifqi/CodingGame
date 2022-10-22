using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject levelSelectPanel;
    //public GameObject settingsPanel;

    public void PlayButton()
    {
        levelSelectPanel.SetActive(!levelSelectPanel.activeSelf);
        levelSelectPanel.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void SettingButton()
    {
        //settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

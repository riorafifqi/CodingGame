using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static string playerUsername;
    [SerializeField] TMP_InputField nameInputField;

    public void OnClickApplyName()
    {
        playerUsername = nameInputField.text;
        nameInputField.text = "";
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.menu));
    }
}

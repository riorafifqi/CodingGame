using JetBrains.Annotations;
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
        NameRandomizer();
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.menu));
    }

    private void NameRandomizer()
    {
        string numbers = "0123456789";
        string numberYield = "";
        for (int i = 0; i < 6; i++)
        {
            numberYield = numberYield + numbers[Random.Range(0, 10)];
        }
        playerUsername = "Player_" + numberYield;
    }
}

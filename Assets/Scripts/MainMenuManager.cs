using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static string playerUsername;
    public static int skinIndex;

    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_Text nameText;

    public GameObject profileSkinTab;
    public Image profileSkinImage;
    public GameObject leftButton;
    public GameObject rightButton;
    public Sprite[] skins;

    public void OnClickApplyName()
    {
        if (nameInputField.gameObject.activeSelf)
        {
            nameInputField.gameObject.SetActive(false);
            nameText.gameObject.SetActive(true);
            
            if (nameInputField.text != "")
            {
                playerUsername = nameInputField.text;
                nameText.text = playerUsername;
            }
        }
        else
        {
            nameInputField.gameObject.SetActive(true);
            nameText.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        NameRandomizer();
        nameText.text = playerUsername;

        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.menu));

        skinIndex = 0;
        profileSkinImage.sprite = skins[skinIndex];
    }

    private void Update()
    {
        profileSkinImage.sprite = skins[skinIndex];
        if (skinIndex <= 0)
        {
            leftButton.SetActive(false);
        }
        else
            leftButton.SetActive(true);

        if (skinIndex >= skins.Length - 1)
        {
            rightButton.SetActive(false);
        }
        else
            rightButton.SetActive(true);
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

    public void OnClickProfile()
    {
        profileSkinTab.SetActive(true);
    }

    public void OnClickProfileLeft()
    {
        skinIndex -= 1;
        if (skinIndex <= 0)
            skinIndex = 0;            
    }

    public void OnClickProfileRight()
    {
        skinIndex += 1;
        if (skinIndex >= skins.Length - 1)
            skinIndex = skins.Length - 1;
    }
}

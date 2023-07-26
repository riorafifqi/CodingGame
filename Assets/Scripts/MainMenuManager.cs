using JetBrains.Annotations;
using System.IO;
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

    public void OnClickApplyName()
    {
        if (nameInputField.gameObject.activeSelf)
        {
            nameInputField.gameObject.SetActive(false);
            nameText.gameObject.SetActive(true);
            
            if (nameInputField.text != "")
            {
                PlayerPrefs.SetString("Name", nameInputField.text);
                nameText.text = PlayerPrefs.GetString("Name");
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
        string tempName = "";
        if (!PlayerPrefs.HasKey("Name"))
            tempName = NameRandomizer();
        else
            tempName = PlayerPrefs.GetString("Name");
        
        nameText.text = tempName;
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.menu));

        string folderName = "CypherCodeCustoms";
        string path = Path.Combine(Application.persistentDataPath, folderName);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private string NameRandomizer()
    {
        string numbers = "0123456789";
        string numberYield = "";
        for (int i = 0; i < 6; i++)
        {
            numberYield = numberYield + numbers[Random.Range(0, 10)];
        }
        
        return "Player_" + numberYield;
    }
}

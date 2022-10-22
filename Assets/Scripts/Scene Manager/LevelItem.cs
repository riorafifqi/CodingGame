using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public Level level;
    public TMP_Text text;
    public Image icon;
    public Sprite[] iconSprite;
    public GameObject selected;

    public LevelInspector inspector;

    Color dark;
    private void Start()
    {
        ColorUtility.TryParseHtmlString("00002E", out dark);
        UpdateUI();
    }

    public void UpdateUI()
    {
        text.text = level.sceneName;
        IfSelected();
        SetIcon();
    }

    void IfSelected()
    {
        if (level == GetComponentInParent<LevelSelectManager>().selectedLevel)
        {
            selected.SetActive(true);
            text.color = dark;
        }
        else
        {
            selected.SetActive(false);
            text.color = Color.white;
        }
    }

    void SetIcon()
    {
        if (level.scores.Length > 0)
            icon.sprite = iconSprite[1];
        else
            icon.sprite = iconSprite[0];
    }

    public void SelectLevel()
    {
        GetComponentInParent<LevelSelectManager>().selectedLevel = level;
        FindObjectOfType<LevelInspector>().UpdateUI();
    }
}

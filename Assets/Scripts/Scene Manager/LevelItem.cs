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

    private void Start()
    {
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
            text.text = "<color=#000000>" + text.text + "</color>";
        }
        else
        {
            selected.SetActive(false);
        }
    }

    void SetIcon()
    {
        if (level.status == Status.locked)
            icon.sprite = iconSprite[2];
        else
        if (level.scores[0].totalLine < 1)
            icon.sprite = iconSprite[0];
        else
            icon.sprite = iconSprite[1];
    }

    public void SelectLevel()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.menu_selection));
        GetComponentInParent<LevelSelectManager>().selectedLevel = level;
        FindObjectOfType<LevelInspector>().UpdateUI();
    }
}

using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public Level selectedLevel;

    public ScenesData levelDatabase;
    public GameObject levelItemPrefab;


    Color dark;
    private void Awake()
    {
        ColorUtility.TryParseHtmlString("00002E", out dark);
        selectedLevel = levelDatabase.levels[0];
        UpdateList();
        UpdateSelected();
    }

    public void UpdateList()
    {
        foreach (var oldLevel in GetComponentsInChildren<LevelItem>())
        {
            Destroy(oldLevel.gameObject);
        }

        foreach (var level in levelDatabase.levels)
        {
            LevelItem temp = Instantiate(levelItemPrefab, transform).GetComponent<LevelItem>();
            temp.level = level;
        }
    }

    public void UpdateSelected()
    {
        foreach (var temp in GetComponentsInChildren<LevelItem>())
        {
            if (temp.level != selectedLevel)
            {
                temp.selected.SetActive(false);
                temp.text.color = Color.white;
            }
            else
            {
                temp.selected.SetActive(true);
                temp.text.color = Color.black;
            }
        }
    }
}

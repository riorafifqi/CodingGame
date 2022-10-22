using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public Level selectedLevel;

    public ScenesData levelDatabase;
    public GameObject levelItemPrefab;

    private void Awake()
    {
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
                temp.selected.SetActive(false);
            else
                temp.selected.SetActive(true);
        }
    }
}

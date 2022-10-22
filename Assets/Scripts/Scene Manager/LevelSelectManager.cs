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
}

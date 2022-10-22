using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInspector : MonoBehaviour
{
    public TMP_Text title;

    public GameObject levelObjectList;
    public GameObject levelObjectPrefab;

    public TMP_Text score;

    public Image thumbnail;

    public GameObject playButton;
    public GameObject locked;

    Level level;
    LevelSelectManager lsm;

    private void Awake()
    {
        lsm = FindObjectOfType<LevelSelectManager>();
        level = lsm.selectedLevel;
        UpdateUI();
    }

    public void UpdateUI()
    {
        thumbnail.sprite = level.levelThumbnail;
        title.text = level.sceneName;



        LockLevel();
        UpdateScore();
    }

    public void UpdateScore()
    {
        score.text = "Name : " + level.scores[0].name + "\n"
                   + "Total Line : " + level.scores[0].totalLine + "\n"
                   + "Time : " + level.scores[0].time;
    }

    void LockLevel()
    {
        if (level.status == Status.locked)
        {
            playButton.SetActive(false);
            locked.SetActive(true);
        }
        else
        {
            playButton.SetActive(true);
            locked.SetActive(false);
        }
    }
}

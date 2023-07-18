using Mono.Cecil;
using System;
using System.Linq;
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

    public GameObject[] objectPrefabsList;

    Level level;
    LevelSelectManager lsm;

    private void Start()
    {
        lsm = FindObjectOfType<LevelSelectManager>();
        level = lsm.selectedLevel;
        UpdateUI();
    }

    public void UpdateUI()
    {
        level = lsm.selectedLevel;
        thumbnail.sprite = level.levelThumbnail;
        title.text = level.sceneName;

        lsm.UpdateList();
        LockLevel();
        UpdateObjects();
        UpdateScore();
    }

    void UpdateObjects()
    {
        
        foreach (var oldObject in levelObjectList.GetComponentsInChildren<LevelObjectDataItem>())
        {
            Destroy(oldObject.gameObject);
        }

        foreach (var newObject in level.objectList)
        {
            LevelObjectDataItem o = Instantiate(levelObjectPrefab, levelObjectList.transform).GetComponent<LevelObjectDataItem>();
            foreach (var prefab in objectPrefabsList)
            {
                if (prefab.name.Contains(newObject.obj.ToString()))
                {
                    o.prefab = prefab;
                }
            }
            o.UpdateText(newObject.count);
            o.UpdateThumbnail();
        }
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

    public void PlayLevel()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.confirm));
        SoundManager.Instance.StopMusic();
        lsm.levelDatabase.LoadLevelWithLevel(level);
    }
}

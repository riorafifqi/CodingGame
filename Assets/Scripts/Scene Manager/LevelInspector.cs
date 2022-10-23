using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInspector : MonoBehaviour
{
    public TMP_Text title;

    public GameObject levelObjectList;
    public GameObject levelObjectPrefab;
    public Sprite[] objectIcon;

    public TMP_Text score;

    public Image thumbnail;

    public GameObject playButton;
    public GameObject locked;

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
        //UpdateObjects();
        UpdateScore();
    }

    void UpdateObjects()
    {
        String[] ObjectsList = level.shortDesc.Split('|');

        foreach (var oldObject in levelObjectList.GetComponentsInChildren<LevelObjectDataItem>())
        {
            Destroy(oldObject.gameObject);
        }

        foreach (String obj in ObjectsList)
        {
            if (obj.Contains("Tutorial"))
                title.text = level.sceneName + " - " + obj;
            else
            {
                LevelObjectDataItem o = Instantiate(levelObjectPrefab, levelObjectList.transform).GetComponent<LevelObjectDataItem>();
                o.text.text = obj;
            }
            //if (obj.Contains("Virus"))
            //{
            //    o.icon.sprite = objectIcon[0];
            //}

            //if (obj.Contains("Switch"))
            //{
            //    o.icon.sprite = objectIcon[1];
            //}

            //if (obj.Contains("Corrupt"))
            //{
            //    o.icon.sprite = objectIcon[2];
            //}

            //if (obj.Contains("Jump"))
            //{
            //    o.icon.sprite = objectIcon[3];
            //}

            //if (obj.Contains("Move"))
            //{
            //    o.icon.sprite = objectIcon[4];
            //}

            //if (obj.Contains("Laser"))
            //{
            //    o.icon.sprite = objectIcon[5];
            //}

            Debug.Log(obj);
        }
        Debug.Log(ObjectsList.Length);
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

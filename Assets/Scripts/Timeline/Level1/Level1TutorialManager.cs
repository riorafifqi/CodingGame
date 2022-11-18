using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TutorialManager : MonoBehaviour
{
    public List<GameObject> listGO;
    public Level level;

    public GameObject tutorial;

    private void Awake()
    {
        if (level.scores[0].time > 0)
            tutorial.SetActive(false);
    }

    public void DisableGO()
    {
        foreach(var i in listGO)
        {
            i.SetActive(false);
        }
    }

    public void EnableGO()
    {
        foreach(var i in listGO)
        {
            i.SetActive(true);
        }
    }
}

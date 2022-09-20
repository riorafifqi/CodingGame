using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isVirusGone;
    public List<GameObject> Viruses;
    // Start is called before the first frame update
    void Start()
    {
        isVirusGone = false;
        Viruses.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        /*Viruses = GameObject.FindGameObjectsWithTag("Enemy");*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Viruses.Count == 0)
        {
            isVirusGone = true;
        }
    }
}

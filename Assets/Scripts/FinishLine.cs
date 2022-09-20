using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if(gameManager.isVirusGone)
            {
                Debug.Log("You Win!");
            }
            else
            {
                Debug.Log("There's still virus left");
            }
        }
    }
}

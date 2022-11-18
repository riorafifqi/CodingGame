using System.Collections.Generic;
using UnityEngine;

public class GameManagerMultiplayer : GameManager
{
    private void Awake()
    {
        commandManager = GetComponent<CommandManagerMultiplayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isVirusGone = false;
        Viruses.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        interactables.AddRange(GameObject.FindGameObjectsWithTag("Interactable"));
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckVirus();
    }
}

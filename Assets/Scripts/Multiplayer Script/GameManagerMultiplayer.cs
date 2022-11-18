using System.Collections.Generic;
using UnityEngine;

public class GameManagerMultiplayer : MonoBehaviour
{
    public bool isVirusGone;
    public List<GameObject> Viruses;
    public List<GameObject> interactables; 

    public List<string> legalCommands;

    [HideInInspector] public CommandManagerMultiplayer commandManager;
    public Level levelData;

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

    public void SetHighscore()
    {
        if (commandManager.stopwatch.GetTime() <= levelData.scores[0].time || levelData.scores[0].time == 0)
        {
            levelData.scores[0].time = commandManager.stopwatch.GetTime();
        }

        if (commandManager.console.lineCount <= levelData.scores[0].totalLine || levelData.scores[0].totalLine == 0)
        {
            levelData.scores[0].totalLine = commandManager.console.lineCount;
        }
    }

    public void CheckVirus()
    {
        foreach (GameObject virus in Viruses)
        {
            Enemy virusScript = virus.GetComponent<Enemy>();
            if (!virusScript.isDead)
            {
                return;
            }
        }

        isVirusGone = true;
    }

    public void ResetLevel()
    {
        if (commandManager.view)
            return;

        isVirusGone = false;
        foreach (GameObject virus in Viruses)
        {
            virus.GetComponent<Enemy>().isDead = false;
            virus.SetActive(true);
        }

        commandManager.console.ResetCommand();
        commandManager.currentCommandIndex = 0;

        foreach (GameObject interact in interactables)
        {
            interact.GetComponent<Push>().ResetPosition();
        }

        commandManager.movement.ResetPosition();
    }
}
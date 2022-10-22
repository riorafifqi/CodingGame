using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isVirusGone;
    public List<GameObject> Viruses;
    public List<string> legalCommands;

    [HideInInspector] public CommandManager commandManager;
    public Level levelData;

    private void Awake()
    {
        commandManager = GetComponent<CommandManager>();
    }

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

    public void SetHighscore()
    {
        if (commandManager.stopwatch.GetTime() <= levelData.scores[0].time)
        {
            levelData.scores[0].time = commandManager.stopwatch.GetTime();
        }

        if (commandManager.console.lineCount <= levelData.scores[0].totalLine)
        {
            levelData.scores[0].totalLine = commandManager.console.lineCount;
        }
    }
}

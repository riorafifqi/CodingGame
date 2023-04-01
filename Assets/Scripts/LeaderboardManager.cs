using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject playerScorePrefab;
    [SerializeField] private Button timeButton;
    [SerializeField] private Button lineButton;
    [SerializeField] private GameObject entryParent;
    string levelName;

    private DatabaseHandler databaseHandler;

    private void Awake()
    {
        timeButton.onClick.AddListener(SortByTime);
        lineButton.onClick.AddListener(SortByLine);
        databaseHandler = FindObjectOfType<DatabaseHandler>();
    }

    private void OnEnable()
    {
        levelName = FindObjectOfType<LevelSelectManager>().selectedLevel.sceneName;
        SortByTime();
    }
    private void OnDisable()
    {
        levelName = "";
        foreach (Transform entry in entryParent.transform)
        {
            Debug.Log("Destroyed");
            Destroy(entry.gameObject);
        }
    }

    private void DisplayList(List<PlayerScore> playerScores)
    {
        foreach (ScoreEntry entry in entryParent.GetComponentsInChildren<ScoreEntry>())
        {
            Debug.Log("Delete " + entry.name);
            entry.DeleteEntry();
        }

        for (int i = 0; i < 10; i++)
        {
            if (playerScores[i] == null)
            {
                continue;
            }

            ScoreEntry scoreEntry = Instantiate(playerScorePrefab, entryParent.transform).GetComponent<ScoreEntry>();
            int number = i + 1;
            scoreEntry.InsertEntry(number.ToString(), playerScores[i].name, playerScores[i].time.ToString(), playerScores[i].line.ToString());
        }
    }

    private void SortByLine()
    {
        databaseHandler.GetScores(levelName, playerScores =>
        {
            List<PlayerScore> sortedList = playerScores.OrderBy(o => o.line).ToList();
            DisplayList(sortedList);
        });
    }

    private void SortByTime()
    {
        databaseHandler.GetScores(levelName, playerScores =>
        {
            List<PlayerScore> sortedList = playerScores.OrderBy(o => o.time).ToList();
            DisplayList(sortedList);
        });
    }

}

using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

public class PlayerScore
{
    public string name { get; set; }
    public float time { get; set; }
    public int line { get; set; }
}

public class DatabaseHandler : MonoBehaviour
{
    [SerializeField] string databaseUrl;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!PlayerPrefs.HasKey("UID"))
        {
            PlayerPrefs.SetString("UID", System.Guid.NewGuid().ToString());
        }
    }

    // used for after game
    public void PostScore(string levelName, string name, float time, int line)
    {
        string url = databaseUrl + "leaderboards/" + levelName + "/" + PlayerPrefs.GetString("UID") + ".json";

        // Send a GET request to retrieve the current best time (if any)
        RestClient.Get(url).Then(response =>
        {
            if (response.StatusCode == ((long)HttpStatusCode.OK))
            {
                // Parse the current best time (if any)
                float currentBestTime = float.MaxValue;
                if (!string.IsNullOrEmpty(response.Text))
                {
                    try
                    {
                        PlayerScore currentPlayerScore = JsonConvert.DeserializeObject<PlayerScore>(response.Text);
                        if (currentPlayerScore != null)
                        {
                            currentBestTime = currentPlayerScore.time;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to parse player {name}'s score: {e}");
                    }
                }

                // Only update the best time if the new time is less than the current best time
                if (time < currentBestTime)
                {
                    PlayerScore newPlayerScore = new PlayerScore();
                    newPlayerScore.time = time;
                    newPlayerScore.name = name;
                    newPlayerScore.line = line;

                    // Send a PUT request to update the database
                    RestClient.Put(url, newPlayerScore).Then(putResponse =>
                    {
                        if (putResponse.StatusCode == ((long)HttpStatusCode.OK))
                        {
                            Debug.Log($"Player {name}'s best time updated to {time}");
                        }
                        else
                        {
                            Debug.LogError($"Failed to update player {name}'s best time: {putResponse.Error}");
                        }
                    }).Catch(error =>
                    {
                        Debug.LogError($"Failed to send PUT request to update player {name}'s best time: {error}");
                    });
                }
            }
            else if (response.StatusCode == ((long)HttpStatusCode.NotFound))
            {
                // New UID, create new entry with best time
                PlayerScore newPlayerScore = new PlayerScore();
                newPlayerScore.time = time;
                newPlayerScore.name = name;
                newPlayerScore.line = line;

                // Send a PUT request to create a new entry in the database
                RestClient.Put(url, newPlayerScore).Then(putResponse =>
                {
                    if (putResponse.StatusCode == ((long)HttpStatusCode.OK))
                    {
                        Debug.Log($"New player {name} registered with best time {time}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to register new player {name}: {putResponse.Error}");
                    }
                }).Catch(error =>
                {
                    Debug.LogError($"Failed to send PUT request to register new player {name}: {error}");
                });
            }
            else
            {
                Debug.LogError($"Failed to retrieve player {name}'s best time: {response.Error}");
            }
        }).Catch(error =>
        {
            Debug.LogError($"Failed to send GET request to retrieve player {name}'s best time: {error}");
        });
    }

    public void GetScores(string levelName, Action<List<PlayerScore>> callback)
    {
        string url = databaseUrl + "leaderboards/" + levelName + ".json";
        Debug.Log(url);

        RestClient.Get(url).Then(response =>
        {
            var responseJson = response.Text;

            /*Debug.Log(response.Text);*/
            Dictionary<string, PlayerScore> playerScores = JsonConvert.DeserializeObject<Dictionary<string, PlayerScore>>(responseJson);
            List<PlayerScore> playerScoreList = playerScores.Values.ToList();
            foreach (var score in playerScoreList)
            {
                Debug.Log($"Name : {score.name} Time : {score.time} Line : {score.line} ");
            }

            callback(playerScoreList);
        });
    }
}

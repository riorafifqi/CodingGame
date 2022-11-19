using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class GameManagerMultiplayer : GameManager
{
    CommandManagerMultiplayer commandManagerMultiplayer;

    public float timeRemaining = 180f;
    bool timeIsRunning;
    public TMP_Text timeText;

    public MovementMultiplayer winningPlayer;

    int virusCount = 0;
    private void Awake()
    {
        commandManagerMultiplayer = GetComponent<CommandManagerMultiplayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isVirusGone = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckVirus();

        Timer();
    }

    public override void CheckVirus()
    {
        if (commandManagerMultiplayer.movementMine.virusKill >= virusCount)
        {
            isVirusGone = true;
            
        }
    }

    [PunRPC]
    public void StartTimer()
    {
        timeRemaining = 180f;
        timeIsRunning = true;
    }

    [PunRPC]
    public void Timer()
    {
        if (timeIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                // time run out
                timeRemaining = 0f;
                timeIsRunning = false;
                if (commandManagerMultiplayer.view.IsMine)
                {
                    commandManagerMultiplayer.commandManagerView.RPC("StartCommandRPC", RpcTarget.All);
                }
            }
        }
        DisplayTime(timeRemaining);
    }

    public void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    public void TimesRunOut()
    {
        timeRemaining = 5f;
    }

    public void GameOver()
    {
        if(winning)
    }
}

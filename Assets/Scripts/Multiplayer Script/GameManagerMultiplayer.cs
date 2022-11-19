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
    public bool isEveryoneStopMoving;

    public int virusCount = 0;
    public WinPanelManager winPanelManager;

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

        if(commandManagerMultiplayer.view.IsMine)
            commandManagerMultiplayer.view.RPC("CheckIsMoving", RpcTarget.All);

        if (commandManagerMultiplayer.isStarting)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.ContainsKey("IsMoving"))
                {
                    if ((bool)player.CustomProperties["IsMoving"] == true)   // if someone is still moving
                    {
                        Debug.Log("Someone is still moving");
                        isEveryoneStopMoving = false;
                        return;
                    }
                }
                isEveryoneStopMoving = true;
                Debug.Log("Everybody has stop moving");
            }
        }

        commandManagerMultiplayer.commandManagerView.RPC("GameOver", RpcTarget.All);
        
    }

    public bool CheckVirus(int killCount)
    {
        if (killCount >= virusCount)
        {
            return true;
        }
        else
        {
            return false;
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

    [PunRPC]
    public void GameOver()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            MovementMultiplayer movementMulti = player.GetComponent<MovementMultiplayer>();
            if (movementMulti.isOnFinishLine)
            {
                // Check if all virus death on it's arena
                if (CheckVirus(movementMulti.virusKill))
                {
                    // show panel
                    winningPlayer = movementMulti;
                    commandManagerMultiplayer.commandManagerView.RPC("ShowWinningPanel", RpcTarget.All);
                }                
            }
            else if (isEveryoneStopMoving)
            {
                commandManagerMultiplayer.commandManagerView.RPC("ShowWinningPanel", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void ShowWinningPanel()
    {
        winPanelManager.OpenWinPanel();
        if (winningPlayer == null)
        {
            winPanelManager.SetStatus("Draw!");
        }
        else
        {
            if (winningPlayer == commandManagerMultiplayer.movementMine)
            {
                winPanelManager.SetStatus("You Win!");
            }
            else if (winningPlayer != commandManagerMultiplayer.movementMine)
            {
                winPanelManager.SetStatus("You Lose!");
            }
        }

    }

    public void DisconnectFromServer()
    {
        PhotonNetwork.Disconnect();
    }
}

using Unity.Netcode;
using UnityEngine;

public class FinishLine : NetworkBehaviour
{
    GameManager gameManager;
    DatabaseHandler leaderboard;
    WinPanelManager winPanel;

    Material[] lampMat;

    private void Awake()
    {
        winPanel = GameObject.FindObjectOfType<WinPanelManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (!gameManager)
            gameManager = FindObjectOfType<GameManagerMultiplayer>();
        lampMat = GameObject.Find("BezierCurve.007").GetComponent<Renderer>().materials;
        ChangeMat(lampMat[4], Color.red);
        ChangeMat(lampMat[1], Color.red * 10);

        leaderboard = FindObjectOfType<DatabaseHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            bool isWinning = other.GetComponent<Movement>().GetFinishStatus();      // get win status from colliding player
            if (isWinning)
            {
                Debug.Log("You Win!");

                int lineCount = gameManager.commandManager.console.lineCount;
                float timeCount = gameManager.commandManager.stopwatch.GetTime();

                //gameManager.SetGameFinish(true);

                if(MultiplayerFlowManager.playMultiplayer)
                    ShowEndPanelServerRPC();    // Get win status from player, whether they're colliding or not, and send it to Server
                else
                {
                    // Singleplayer
                    winPanel.SetLineCount(lineCount);
                    winPanel.SetTime(timeCount);
                    winPanel.OpenSucceedPanel();

                    // Store to database
                    leaderboard.PostScore(gameManager.GetLevelName(), PlayerPrefs.GetString("Name"), timeCount, lineCount);

                    gameManager.commandManager.stopwatch.StopStopwatch();
                    gameManager.SetHighscore();
                }
            }
            else
            {
                Debug.Log("There's still virus left");
            }
        }
    }

    public void activateFinish()
    {
        ChangeMat(lampMat[4], Color.green);
        ChangeMat(lampMat[1], Color.green * 10);
    }

    void ChangeMat(Material mat, Color color)
    {
        mat.SetColor("_EmissionColor", color);
        mat.SetColor("_Color", color);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShowEndPanelServerRPC()
    {
        ShowEndPanelClientRPC();      // Server then broadcast it to all client
    }

    [ClientRpc]
    void ShowEndPanelClientRPC()
    {
        gameManager.SetGameFinish(true);
        if (Movement.LocalInstance.GetFinishStatus())
        {
            winPanel.OpenWinPanel();
        }
        else
        {
            winPanel.OpenLosePanel();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerGameManager : NetworkBehaviour
{
    // Camera
    [SerializeField] CameraController localCameraController;
    [SerializeField] CameraController otherPlayerCameraController;
    float transitionDuration = 1.5f;
    [SerializeField] RectTransform consolePanel;

    // Kill related
    GameManager gameManager;
    Movement[] players;
    float playerCount = 0;

    private void OnEnable()
    {
        GameplayEvent.OnStartRunningE += SplitScreen;
    }

    private void OnDisable()
    {
        GameplayEvent.OnStartRunningE -= SplitScreen;
    }

    private void Awake()
    {
        Movement.OnAnyPlayerSpawned += Movement_OnAnyPlayerSpawned;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        StartCoroutine(CheckDraw());
    }

    private void Movement_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        AssignCamera();
    }

    private void AssignCamera()
    {
        players = FindObjectsOfType<Movement>();
        foreach (Movement player in players)
        {
            if (player == Movement.LocalInstance)
                localCameraController.SetTarget(Movement.LocalInstance.transform);
            else
                otherPlayerCameraController.SetTarget(player.transform);
        }
    }

    public void SplitScreen()
    {
        StartCoroutine(SplitScreenCoroutine());
    }

    private IEnumerator SplitScreenCoroutine()
    {
        Camera localCamera = localCameraController.GetComponentInChildren<Camera>();
        Camera otherCamera = otherPlayerCameraController.GetComponentInChildren<Camera>();

        float t = 0f;
        while (t < transitionDuration)
        {
            t += Time.deltaTime;

            // Calculate the new viewport rect values for each camera
            Rect player1ViewportRect = new Rect(0f, 0f, Mathf.Lerp(1f, 0.5f, t / transitionDuration), 1f);
            Rect player2ViewportRect = new Rect(Mathf.Lerp(1f, 0.5f, t / transitionDuration), 0f, 1f, 1f);
            Vector2 consoleOutPosition = new Vector2(Mathf.Lerp(consolePanel.transform.position.x, -Screen.width, t / transitionDuration), consolePanel.transform.position.y);

            // Apply the new viewport rect values to the cameras
            localCamera.rect = player1ViewportRect;
            otherCamera.rect = player2ViewportRect;
            consolePanel.position = consoleOutPosition;

            yield return null;
        }

        // Ensure the final viewport rect values are set correctly
        localCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
        otherCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
    }

    private IEnumerator CheckDraw()
    {
        Debug.Log("Check Draw is Running");

        if ((players == null && playerCount < 2) || gameManager.GetGameFinish())
            yield break;

        foreach (Movement player in players)
        {
            // If all player is still moving, return
            if (!player.GetAllCommandExecuted())
                yield break;
        }

        yield return new WaitForSeconds(1f);

        CheckKillCount();
    }

    private void CheckKillCount()
    {
        FinishLine finishLine = FindObjectOfType<FinishLine>();
        if (players[0].GetKillCount() > players[1].GetKillCount())
        {
            players[0].SetFinishStatus(true);       // Finish status use to determine who win            
        }
        else if (players[0].GetKillCount() < players[1].GetKillCount())
        {
            players[1].SetFinishStatus(true);
        }
        else
        {
            // show draw panel
            OpenDrawPanelClientRpc();
            gameManager.SetGameFinish(true);
            return;
        }

        finishLine.ShowEndPanelServerRPC();
    }

    [ClientRpc]
    private void OpenDrawPanelClientRpc()
    {
        FindObjectOfType<WinPanelManager>().OpenDrawPanel();
    }
}

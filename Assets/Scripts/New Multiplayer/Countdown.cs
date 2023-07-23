using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Countdown : NetworkBehaviour
{
    TMP_Text countdownUI;
    [SerializeField] float duration;
    float timeLeft;
    int playerCount = 0;

    private void OnEnable()
    {
        Movement.OnAnyPlayerSpawned += Movement_OnAnyPlayerSpawned;
    }

    private void OnDisable()
    {
        Movement.OnAnyPlayerSpawned -= Movement_OnAnyPlayerSpawned;
    }

    private void Awake()
    {
        countdownUI = GetComponentInChildren<TMP_Text>();
    }

    private void Movement_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        playerCount++;

        if (playerCount >= 2)
            StartCoroutine(CountdownStart());
    }

    IEnumerator CountdownStart()
    {
        if (!IsServer)
            yield break; 

        timeLeft = duration;
        while (timeLeft > 0)
        {
            DisplayCountdownClientRPC(timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        DisplayCountdownClientRPC(timeLeft);

        // time's up!
        TimeIsUpClientRPC();
    }

    [ClientRpc]
    private void DisplayCountdownClientRPC(float time)
    {
        countdownUI.text = time.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DecreaseCountdownServerRPC()
    {
        if (timeLeft <= 6f)
            return;

        timeLeft = 6f;
    }

    [ClientRpc]
    private void TimeIsUpClientRPC()
    {
        GameplayEvent.OnTimeIsUp();
    }
}

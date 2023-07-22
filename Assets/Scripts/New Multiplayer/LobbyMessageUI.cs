using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject container;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        LobbyCypherCode.Instance.OnCreateLobbyStarted += LobbyCypherCode_OnCreateLobbyStarted;
        LobbyCypherCode.Instance.OnCreateLobbyFailed += LobbyCypherCode_OnCreateLobbyFailed;
        LobbyCypherCode.Instance.OnJoinStarted += LobbyCypherCode_OnJoinStarted;
        LobbyCypherCode.Instance.OnJoinFailed += LobbyCypherCode_OnJoinFailed;
        LobbyCypherCode.Instance.OnQuickJoinFailed += LobbyCypherCode_OnQuickJoinFailed;

    }

    private void LobbyCypherCode_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could not find a lobby to quick join");
    }

    private void LobbyCypherCode_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed To Join Lobby");
    }

    private void LobbyCypherCode_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining Lobby....");
    }

    private void LobbyCypherCode_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed To Create Lobby!");

        /*if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        } else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }*/
    }

    private void LobbyCypherCode_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Lobby....");
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Hide()
    {
        container.gameObject.SetActive(false);
    }

    private void Show()
    {
        container.gameObject.SetActive(true);
    }
}

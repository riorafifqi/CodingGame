using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerFlowManager : NetworkBehaviour
{
    public static MultiplayerFlowManager Instance { get; private set; }

    [SerializeField] GameObject[] characterDatabase;
    private NetworkList<PlayerData> playerDataNetworkList;

    public event EventHandler OnPlayerDataNetworkListChanged;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerDataNetworkList = new NetworkList<PlayerData>();

        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;

        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
        });
    }

    public GameObject GetCharacterPrefab(int index)
    {
        return characterDatabase[index];
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }
}

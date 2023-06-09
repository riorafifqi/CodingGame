using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerFlowManager : NetworkBehaviour
{
    public static MultiplayerFlowManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}

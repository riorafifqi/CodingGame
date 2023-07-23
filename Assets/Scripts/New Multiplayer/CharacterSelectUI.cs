using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button readyButton;
    
    [SerializeField] private TextMeshProUGUI lobbyCode;
    [SerializeField] private TextMeshProUGUI lobbyName;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            LobbyCypherCode.Instance.LeaveLobby();
            LobbyCypherCode.Instance.DeleteLobby();

            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("RevampedMainMenu");
        });

        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        });
    }

    private void Start()
    {
        Lobby lobby = LobbyCypherCode.Instance.GetLobby();

        lobbyName.text = "Lobby Name : " + lobby.Name;
        lobbyCode.text = lobby.LobbyCode;
    }

}

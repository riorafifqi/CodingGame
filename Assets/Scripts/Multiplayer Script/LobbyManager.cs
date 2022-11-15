using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    string roomCode;

    public TMP_Text roomCodeUI;
    public TMP_InputField enterCodeInput;

    string legalChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public void OnClickCreate()
    {
        roomCode = "";
        // Generate room unique code
        for (int i = 0; i < 5; i++)
        {
            roomCode += legalChar[Random.Range(0, legalChar.Length)];
        }
        
        // Specify Room Options (for max player, etc etc)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        // Create Room, use unique code for joining later
        PhotonNetwork.CreateRoom(roomCode, roomOptions);
    }

    public void OnClickJoin()
    {
        if(enterCodeInput.text.Length > 0)
            PhotonNetwork.JoinRoom(enterCodeInput.text);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickQuickPlay()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        base.OnCreateRoomFailed(returnCode, message);

        OnClickCreate();    // Create room again
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        
        roomCodeUI.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {        
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);

        // Create new room
        OnClickCreate();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        // Destroy all playerItem in list
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
            return;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            playerItemsList.Add(newPlayerItem);
        }
    }
}

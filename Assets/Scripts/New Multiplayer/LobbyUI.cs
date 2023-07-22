using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button createLobbyButton;
    [SerializeField] Button quickJoinButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button joinButton;
    [SerializeField] TMP_InputField joinInputField;
    [SerializeField] LobbyCreateUI lobbyCreateUI;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            LobbyCypherCode.Instance.LeaveLobby();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene(0);
        });

        createLobbyButton.onClick.AddListener(() =>
        {
            lobbyCreateUI.Show();
        });

        quickJoinButton.onClick.AddListener(() =>
        {
            LobbyCypherCode.Instance.QuickJoin();
        });

        joinButton.onClick.AddListener(() =>
        {
            LobbyCypherCode.Instance.JoinWithCode(joinInputField.text);
        });
    }
}

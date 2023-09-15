using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    private void Awake()
    {
        createPublicButton.onClick.AddListener(() =>
        {
            string lobbyName = lobbyNameInputField.text;

            if (lobbyName == "")
                lobbyName = "Lobby_" + Random.Range(1000, 9999).ToString();

            LobbyCypherCode.Instance.CreateLobby(lobbyName, false);
        });

        createPrivateButton.onClick.AddListener(() =>
        {
            string lobbyName = lobbyNameInputField.text;
            if (lobbyName == "")
                lobbyName = "Lobby_" + Random.Range(1000, 9999).ToString();

            LobbyCypherCode.Instance.CreateLobby(lobbyName, true);
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

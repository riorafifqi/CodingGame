using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        createButton.onClick.AddListener(() =>
       {
           MultiplayerFlowManager.Instance.StartHost();
           NetworkManager.Singleton.SceneManager.LoadScene("CharacterSelectScene", LoadSceneMode.Single);

       });
        joinButton.onClick.AddListener(() =>
        {
            MultiplayerFlowManager.Instance.StartClient();
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button readyButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene("RevampedMainMenu", LoadSceneMode.Single);
        });

        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        });
    }

}

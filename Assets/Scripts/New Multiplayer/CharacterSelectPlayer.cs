using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{

    [SerializeField] private int playerIndex;
    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private GameObject readyMark;
    [SerializeField] private GameObject[] skins;

    private void Start()
    {
        MultiplayerFlowManager.Instance.OnPlayerDataNetworkListChanged += MultiplayerFlowManager_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        UpdatePlayer();
    }

    private void OnDisable()
    {
        MultiplayerFlowManager.Instance.OnPlayerDataNetworkListChanged -= MultiplayerFlowManager_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged -= CharacterSelectReady_OnReadyChanged;
        Hide();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void MultiplayerFlowManager_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    void UpdatePlayer()
    {
        if (MultiplayerFlowManager.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();

            PlayerData playerData = MultiplayerFlowManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);

            readyMark.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            playerNameText.text = playerData.playerName.ToString();
            ChooseSkin(playerData.skinId);
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        /*if (playerModel.transform.childCount == 1)
            return;

        GameObject playerCharacter = MultiplayerFlowManager.Instance.GetCharacterPrefab(skinId);
        Instantiate(playerCharacter, playerModel.transform);*/

        gameObject.SetActive(true);        
    }

    public void Hide()
    {
        /*if (playerModel.transform.childCount > 0)
            Destroy(playerModel.transform.GetChild(0).gameObject);*/

        gameObject.SetActive(false);
    }

    public void ResetCharacter()
    {
        
    }

    public void ChooseSkin(int skinIndex)
    {
        foreach (GameObject skin in skins)
        {
            skin.SetActive(false);
        }

        skins[skinIndex].SetActive(true);
    }
}

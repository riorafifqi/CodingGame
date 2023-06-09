using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{

    [SerializeField] private int playerIndex;
    private void Start()
    {
        MultiplayerFlowManager.Instance.OnPlayerDataNetworkListChanged += MultiplayerFlowManager_OnPlayerDataNetworkListChanged;

        UpdatePlayer();
    }

    private void MultiplayerFlowManager_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        Debug.Log("DataPlayer List changed");
        UpdatePlayer();
    }

    void UpdatePlayer()
    {
        Hide();
        if (MultiplayerFlowManager.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show(int skinId = 0)
    {
        if (this.gameObject.transform.childCount == 1)
            return;

        GameObject playerCharacter = MultiplayerFlowManager.Instance.GetCharacterPrefab(skinId);
        Instantiate(playerCharacter, this.gameObject.transform);
    }

    public void Hide()
    {
        if (this.gameObject.transform.childCount > 0)
            Destroy(this.gameObject.transform.GetChild(0));
    }
}

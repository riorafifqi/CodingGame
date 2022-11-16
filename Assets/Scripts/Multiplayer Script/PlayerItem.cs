using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
    public TMP_Text isReadyText;

    Player player;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    public void SetPlayerInfo(Player player)
    {
        playerName.text = player.NickName;
        this.player = player;
        SetPlayerReadyToFalse();
        //UpdatePlayerItem(player);
    }

    public void SetPlayerReadyToFalse()
    {
        playerProperties["isReady"] = false;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (!player.IsMasterClient)
        {
            if (player.CustomProperties.ContainsKey("isReady"))
            {
                if ((bool)player.CustomProperties["isReady"] == true)
                {
                    isReadyText.gameObject.SetActive(true);
                    isReadyText.text = "Ready";
                }
                else if ((bool)player.CustomProperties["isReady"] == false)
                {
                    isReadyText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            isReadyText.gameObject.SetActive(true);
            isReadyText.text = "Master";
        }
        Debug.Log(player.NickName + " ReADY? : " + player.CustomProperties["isReady"]);
        Debug.Log("who is the master? " + PhotonNetwork.MasterClient);
    }
}

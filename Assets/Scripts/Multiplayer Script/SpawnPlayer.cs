using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject character;

    public Transform[] spawners;

    private void Awake()
    {
        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(false);
        }

        int playerIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        PhotonNetwork.Instantiate(character.name, spawners[playerIndex].position, Quaternion.identity);
    }
}

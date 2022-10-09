using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public static bool isJumpingPlatform;
    public float distance;
    Movement player;

    private void Start()
    {
        player = FindObjectOfType<Movement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player.Jump(distance);
            isJumpingPlatform = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptTile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<Movement>().Death();
        }
    }
}

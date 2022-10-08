using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public float distance;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Jump(distance, other);
        }
    }

    void Jump(float distance, Collider other)
    {
        float maxHeight = 2f;
        float maxDistance = distance;

        var g = Physics.gravity.magnitude;
        var vSpeed = Mathf.Sqrt(2 * g * maxHeight);
        var totalTime = 2 * vSpeed / g;
        var hSpeed = maxDistance / totalTime;

        other.attachedRigidbody.velocity = transform.forward * hSpeed + transform.up * vSpeed;
    }
}

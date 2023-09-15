using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public static bool isJumpingPlatform;
    public float distance;
    Movement player;

    public Vector3 startPos;
    public Vector3 targetPos;

    private void Start()
    {
        player = FindObjectOfType<Movement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Throw(other.transform);
            isJumpingPlatform = true;
        }
    }

    public void Throw(Transform target)
    {
        Debug.Log("Throw called");

        //player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);

        Rigidbody rb = target.GetComponent<Rigidbody>();
        Animator animator = target.GetComponentInChildren<Animator>();

        player.startPos = transform.position;
        player.targetPos += transform.position + transform.forward * distance;

        float maxHeight = 2f;
        float maxDistance = distance;

        var g = Physics.gravity.magnitude;
        var vSpeed = Mathf.Sqrt(2 * g * maxHeight);
        var totalTime = 2 * vSpeed / g;
        var hSpeed = maxDistance / totalTime;

        rb.velocity = transform.forward * hSpeed + transform.up * vSpeed;

        foreach (AnimatorControllerParameter controller in animator.parameters)
        {
            animator.SetBool(controller.name, false);
        }

        GetComponentInChildren<Animator>().SetTrigger("Activate");
        animator.SetBool("Jump", true);
        
        StartCoroutine(player.OnGroundEnter());
    }
}

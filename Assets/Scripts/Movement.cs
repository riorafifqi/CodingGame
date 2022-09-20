using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 playerPositionOnStart;   // Player position when level start
    Quaternion playerRotationOnStart;

    public Vector3 targetPos;
    public Vector3 startPos;

    Quaternion targetRot;
    Quaternion startRot;

    int amount;

    bool isMoving;
    bool isRotating;

    float movingSpeed;
    public float groundedSpeed = 3f;
    public float flyingSpeed = 1f;

    public float turnSpeed = 0.01f;
    public float jumpForce = 10f;

    RaycastHit hitInfo;
    Rigidbody rb;
    [SerializeField] CommandManager commandManager;

    void Start()
    {
        this.transform.position = new Vector3(0, 0.5f, 0);
        rb = transform.GetComponent<Rigidbody>();
        startPos = transform.position;

        playerPositionOnStart = transform.position;
        playerRotationOnStart = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMoving)
        {
            if (Mathf.Abs(transform.position.x - targetPos.x) < 0.1f && Mathf.Abs(transform.position.z - targetPos.z) < 0.1f)   // if finished moving
            {
                //Debug.Log("Achieved");
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                isMoving = false;
                commandManager.NextCommand();
                return;
            }



            transform.position += (targetPos - startPos) * movingSpeed * Time.deltaTime;
            return;
        }

        if (isRotating)
        {
            if(Quaternion.Angle(transform.rotation, targetRot) < 0.1f)      // if Finished rotating
            {
                transform.rotation = targetRot;
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                isRotating = false;
                commandManager.NextCommand();
                return;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSpeed);
            return;
        }

        // Uncomment to Test
        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveForward(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveBackward(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }*/
    }

    public void MoveForward(int amount)
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f) && hitInfo.transform.tag == "Obstacle")
        {
            return;
        }
        else
        {
            targetPos = transform.position + transform.forward * amount;
        }
        //this.amount = amount;
        startPos = transform.position;
        isMoving = true;
        movingSpeed = groundedSpeed;
    }

    public void MoveBackward(int amount)
    {
        if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, 1f) && hitInfo.transform.tag == "Obstacle")
        {
            Debug.Log("There's object");
            return;
        }
        else
        {
            targetPos = transform.position + transform.forward * amount;
        }
        //this.amount = amount;
        targetPos = transform.position + transform.forward * amount * -1;
        startPos = transform.position;
        isMoving = true;
        movingSpeed = groundedSpeed;
    }

    public void RotateLeft()
    {
        targetRot = transform.rotation * Quaternion.Euler(0, -90, 0);
        startRot = this.transform.rotation;
        isRotating = true;
    }

    public void RotateRight()
    {
        targetRot = transform.rotation * Quaternion.Euler(0, 90, 0);
        startRot = this.transform.rotation;
        isRotating = true;
    }

    public void Jump()
    {
        rb.AddForce(new Vector3(0, 1, 0) * jumpForce);
        targetPos = transform.position + transform.forward;
        startPos = transform.position;
        movingSpeed = flyingSpeed;   // default jumping z speed
        isMoving = true;
    }

    public void ResetPosition()
    {
        transform.position = playerPositionOnStart;
        transform.rotation = playerRotationOnStart;
    }
}

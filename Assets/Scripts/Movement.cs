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
    float distToGround;

    bool isMoving;
    bool isRotating;
    bool isPushing;
    public bool isJumping = false;

    float movingSpeed;
    public float groundedSpeed = 3f;
    public float flyingSpeed = 1f;

    public float turnSpeed = 0.01f;
    public float jumpForce = 10f;

    [SerializeField] BoxCollider collider;
    RaycastHit hitInfo;
    Rigidbody rb;
    Animator animator;
    [SerializeField] CommandManager commandManager;

    void Start()
    {
        this.transform.position = new Vector3(0, 0.5f, 0);
        rb = transform.GetComponent<Rigidbody>();
        commandManager = GameObject.Find("Game Manager").GetComponent<CommandManager>();
        animator = FindObjectOfType<Animator>();
        collider = GetComponent<BoxCollider>();

        startPos = transform.position;

        playerPositionOnStart = transform.position;
        playerRotationOnStart = transform.rotation;
        distToGround = collider.bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xVelocity = 5f, yVelocity = 5f;
        Debug.Log(transform.forward * xVelocity + transform.up * yVelocity);

        if (isMoving)
        {
            if (Mathf.Abs(transform.position.x - targetPos.x) < 0.1f && Mathf.Abs(transform.position.z - targetPos.z) < 0.1f)   // if finished moving
            {
                //Debug.Log("Achieved");
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                
                isMoving = false;
                isPushing = false;

                animator.SetBool("Push", false);
                animator.SetBool("Walk", false);

                return;
            }

            // Update startPos everytime player move 1 tile
            /*if ((int)Mathf.Abs(transform.position.x - startPos.x) >= 1f || (int)Mathf.Abs(transform.position.z - targetPos.z) >= 1f)
            {
                startPos.x = (int)transform.position.x;
                startPos.z = (int)transform.position.z;
            }*/


            /*if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f) && (hitInfo.transform.tag == "Obstacle" || hitInfo.transform.tag == "Interactable"))
            {
                transform
                return;
            }*/

            if (transform)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, movingSpeed * Time.deltaTime);

            return;
        }

        if (isRotating)
        {
            if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)      // if Finished rotating
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

        if (IsGrounded() && isJumping)
        {
            Debug.Log("Character grounded");
            isJumping = false;
            animator.SetBool("Jump", false);
            //commandManager.NextCommand();
        }
    }

    public void MoveForward(int amount)
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f) && (hitInfo.transform.tag == "Obstacle"))
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

        if (!isPushing)
            animator.SetBool("Walk", true);
    }

    public void MoveBackward(int amount)
    {
        if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, 1f) && (hitInfo.transform.tag == "Obstacle" || hitInfo.transform.tag == "Interactable"))
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

    public void Jump(float distance = 1f)
    {
        /*rb.AddForce(new Vector3(0, 1, 0) * jumpForce);
        targetPos = transform.position + transform.forward;
        startPos = transform.position;
        movingSpeed = flyingSpeed;   // default jumping z speed
        
        isMoving = true;
        isJumping = true;
        animator.SetBool("Jump", true);*/

        float maxHeight = 2f;
        float maxDistance = distance;

        var g = Physics.gravity.magnitude;
        var vSpeed = Mathf.Sqrt(2 * g * maxHeight);
        var totalTime = 2 * vSpeed / g;
        var hSpeed = maxDistance / totalTime;

        rb.velocity = transform.forward * hSpeed + transform.up * vSpeed;

        isJumping = true;
        animator.SetBool("Jump", true);
    }

    public void Push(int amount)
    {
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f);
        Push push = hitInfo.transform.GetComponent<Push>();
        if (push != null)
        {
            isPushing = true;

            push.Pushed(amount);
            MoveForward(amount);
            animator.SetBool("Push", true);
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround);
    }

    public void Press()
    {
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f);
        Interactable interactable = hitInfo.transform.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    public void ResetPosition()
    {
        transform.position = playerPositionOnStart;
        transform.rotation = playerRotationOnStart;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * (collider.bounds.extents.y + 0.1f));
    }
}

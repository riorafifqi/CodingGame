using System.Collections;
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
    public bool isJumping;
    public bool isGrounded;

    float movingSpeed;
    public float groundedSpeed = 3f;
    public float flyingSpeed = 1f;

    public float turnDuration = 0.01f;
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
        CheckGround();

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
                
                commandManager.NextCommand();

                return;
            }

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

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnDuration);
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                isJumping = false;
                animator.SetBool("Jump", isJumping);

                if(!JumpPlatform.isJumpingPlatform)
                    commandManager.NextCommand();

                JumpPlatform.isJumpingPlatform = false;
            }

            return;
        }
    }

    public void MoveForward(int amount = 1)
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

    public void MoveBackward(int amount = 1)
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

    public IEnumerator RotateLeft(int index = 1)
    {
        for (int i = 0; i < index; i++)
        {
            float startRotation = transform.eulerAngles.y;
            float endRotation = startRotation - 90.0f;
            float t = 0.0f;
            while (t < turnDuration)
            {
                t += Time.deltaTime;
                float yRotation = Mathf.Lerp(startRotation, endRotation, t / turnDuration);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
                yield return null;
            }
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        commandManager.NextCommand();
    }

    public IEnumerator RotateRight(int index = 1)
    {
        for (int i = 0; i < index; i++)
        {
            float startRotation = transform.eulerAngles.y;
            float endRotation = startRotation + 90.0f;
            float t = 0.0f;
            while (t < turnDuration)
            {
                t += Time.deltaTime;
                float yRotation = Mathf.Lerp(startRotation, endRotation, t / turnDuration);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
                yield return null;
            }
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        commandManager.NextCommand();
    }

    public void Jump(float distance = 1f)
    {
        startPos = transform.position;
        targetPos = transform.position + transform.forward * distance;

        float maxHeight = 2f;
        float maxDistance = distance;

        var g = Physics.gravity.magnitude;
        var vSpeed = Mathf.Sqrt(2 * g * maxHeight);
        var totalTime = 2 * vSpeed / g;
        var hSpeed = maxDistance / totalTime;

        rb.velocity = transform.forward * hSpeed + transform.up * vSpeed;

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

    public void CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isJumping = true;
            isGrounded = false;
        }
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

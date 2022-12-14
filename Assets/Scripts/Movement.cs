using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected Vector3 playerPositionOnStart;   // Player position when level start
    protected Quaternion playerRotationOnStart;

    public Vector3 targetPos;
    public Vector3 startPos;

    protected float distToGround;

    protected bool isMoving;
    protected bool isRotating;
    protected bool isPushing;
    public bool isJumping;
    public bool isGrounded;

    protected float movingSpeed;
    public float groundedSpeed = 3f;
    public float flyingSpeed = 1f;

    public float turnDuration = 0.01f;
    public float jumpForce = 10f;

    [SerializeField] protected BoxCollider boxCollider;
    protected RaycastHit hitInfo;
    protected Rigidbody rb;
    public Animator animator;
    [SerializeField] protected CommandManager commandManager;
    public GameObject explosion;

    void Start()
    {
        //this.transform.position = new Vector3(0, 0.5f, 0);
        rb = transform.GetComponent<Rigidbody>();
        
        commandManager = GameObject.Find("Game Manager").GetComponent<CommandManager>();
        if (commandManager == null)
        {
            commandManager = FindObjectOfType<CommandManagerMultiplayer>();
        }

        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        startPos = transform.position;
        targetPos = startPos;

        playerPositionOnStart = transform.position;
        playerRotationOnStart = transform.rotation;
        distToGround = boxCollider.bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGround();

        if (isMoving)
        {
            if (Mathf.Abs(transform.position.x - targetPos.x) < 0.1f && Mathf.Abs(transform.position.z - targetPos.z) < 0.1f)   // if arrived
            {
                //Debug.Log("Achieved");
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);

                isMoving = false;
                isPushing = false;

                animator.SetBool("Push", false);
                animator.SetBool("Walk", false);
                
                if(isGrounded)    // not falling
                    commandManager.NextCommand();

                return;
            }

            if (isGrounded)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, movingSpeed * Time.deltaTime);
            isJumping = false;

            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                Debug.Log("Is Jumping called");
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

    public virtual void MoveForward(int amount = 1)
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

    public virtual void MoveBackward(int amount = 1)
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

    public virtual IEnumerator RotateLeft(int index = 1)
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

    public virtual IEnumerator RotateRight(int index = 1)
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

    public virtual void Jump(float distance = 1f)
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

    public virtual void Push(int amount)
    {
        Push push = null;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f)) 
            push = hitInfo.transform.GetComponent<Push>();

/*        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && hitInfo.transform.tag == "Obstacle")
        {
            return;
        }*/

        if (push != null)
        {
            isPushing = true;

            push.Pushed(push.transform.position + transform.forward * amount);
            MoveForward(amount);
            animator.SetBool("Push", true);
        }
        else
        {
            commandManager.NextCommand();
            return;
        }
    }

    public virtual void Empty()
    {
        commandManager.NextCommand();
    }

    public virtual IEnumerator Wait(int duration)
    {
        yield return new WaitForSeconds(duration);
        commandManager.NextCommand();
    }

    public virtual void CheckGround()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
        {
            if(hit.collider.tag != "Enemy")
                isGrounded = true;
        }
        else
        {
            isJumping = true;
            isGrounded = false;
        }
    }

    public virtual void Press()
    {
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f);
        Interactable interactable = hitInfo.transform.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    public virtual void ResetPosition()
    {
        isMoving = false;
        isRotating = false;

        transform.position = playerPositionOnStart;
        transform.rotation = playerRotationOnStart;

        startPos = playerPositionOnStart;
        targetPos = playerPositionOnStart;

        this.gameObject.SetActive(true);
    }

    public virtual void Death()
    {
        Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
        this.gameObject.SetActive(false);
        commandManager.console.isFinish = true;
        commandManager.NextCommand();
        return;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * (boxCollider.bounds.extents.y + 0.01f));
    }
}

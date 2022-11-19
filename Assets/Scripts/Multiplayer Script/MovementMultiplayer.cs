using Photon.Pun;
using System.Collections;
using UnityEngine;

public class MovementMultiplayer : Movement
{
    public int virusKill = 0;
    public int currentCommandIndex = 0;
    [HideInInspector] public PhotonView view;

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

        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
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

                    if (isGrounded)    // not falling
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

                    if (!JumpPlatform.isJumpingPlatform)
                        commandManager.NextCommand();

                    JumpPlatform.isJumpingPlatform = false;
                }

                return;
            }
        }
    }

    public override void MoveForward(int amount = 1)
    {
        if (view.IsMine)
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
    }

    public override void MoveBackward(int amount = 1)
    {
        if (view.IsMine)
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
    }

    public override IEnumerator RotateLeft(int index = 1)
    {
        if (view.IsMine)
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
    }

    public override IEnumerator RotateRight(int index = 1)
    {
        if (view.IsMine)
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
        
    }

    public override void Jump(float distance = 1f)
    {
        if (view.IsMine)
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
    }

    public override void Push(int amount)
    {
        if (view.IsMine)
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
    }

    [PunRPC]
    public void PushRPC(int amount)
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

    public override void Empty()
    {
        if(view.IsMine)
            commandManager.NextCommand();
    }

    public override IEnumerator Wait(int duration)
    {
        if (view.IsMine)
        {
            yield return new WaitForSeconds(duration);
            commandManager.NextCommand();
        }
    }

    public override void CheckGround()
    {
        if (view.IsMine)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
            {
                if (hit.collider.tag != "Enemy")
                    isGrounded = true;
            }
            else
            {
                isJumping = true;
                isGrounded = false;
            }
        }
    }

    public override void Press()
    {
        if (view.IsMine)
        {
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f);
            Interactable interactable = hitInfo.transform.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }        
    }

    public override void ResetPosition()
    {
        isMoving = false;
        isRotating = false;

        transform.position = playerPositionOnStart;
        transform.rotation = playerRotationOnStart;

        startPos = playerPositionOnStart;
        targetPos = playerPositionOnStart;

        this.gameObject.SetActive(true);
    }

    public override void Death()
    {
        if (view.IsMine)
        {
            Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
            this.gameObject.SetActive(false);
            commandManager.console.isFinish = true;
            commandManager.NextCommand();
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * (boxCollider.bounds.extents.y + 0.01f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (view.IsMine)
        {
            if (other.transform.tag == "Enemy")
            {
                view.RPC("KillVirus", RpcTarget.All);
            }

            if (other.gameObject.name == "FinishLine")
            {
                if (FindObjectOfType<GameManagerMultiplayer>().isVirusGone)
                {
                    // Winning
                }

            }
        }
    }

    [PunRPC]
    public void KillVirus()
    {
        virusKill++;
    }
}

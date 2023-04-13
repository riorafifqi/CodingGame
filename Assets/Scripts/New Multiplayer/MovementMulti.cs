using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class MovementMulti : Movement
{
    void Start()
    {
        //this.transform.position = new Vector3(0, 0.5f, 0);
        rb = transform.GetComponent<Rigidbody>();

        commandManager = FindObjectOfType<CommandManager>();
        if (commandManager == null)
        {
            commandManager = FindObjectOfType<CommandManagerMultiplayer>();
        }

        StartCoroutine(FetchComponent());

        startPos = transform.position;
        targetPos = startPos;

        playerPositionOnStart = transform.position;
        playerRotationOnStart = transform.rotation;
        distToGround = boxCollider.bounds.extents.y;

        smokeDir = smokeTrail.GetVector3("Velocity");
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
    }

    private void Update()
    {
        if (!isMoving)
        {
            smokeTrail.SetFloat("Spawn Rate", 0);
        }
        else
        {
            smokeTrail.SetFloat("Spawn Rate", 16);
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
        smokeTrail.SetVector3("Velocity", new Vector3(smokeDir.x,smokeDir.y,-smokeDir.z));

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
        smokeTrail.SetVector3("Velocity", new Vector3(smokeDir.x, smokeDir.y, Mathf.Abs(smokeDir.z)));
    }

    public override IEnumerator ForwardMove(int index = 1)
    {
        animator.SetBool("Walk", true);
        for (int i = 0; i < index; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = startPosition + transform.forward;

            float t = 0.0f;
            while (t < groundedSpeed)
            {
                t += Time.deltaTime;
                Debug.Log((int)transform.forward.z);

                // Calculate which axis is affected by transform.forward
                float tempValue, deltaForward = 0;
                if ((int)transform.forward.x != 0)
                {
                    tempValue = Mathf.Lerp(startPosition.x, endPosition.x, t / groundedSpeed);
                    deltaForward = Mathf.Abs(tempValue - transform.position.x);
                }
                else if ((int)transform.forward.z != 0)
                {
                    tempValue = Mathf.Lerp(startPosition.z, endPosition.z, t / groundedSpeed);
                    deltaForward = Mathf.Abs(tempValue - transform.position.z);
                    Debug.Log("toward z");
                }

                //Vector3 tempPos = Vector3.Lerp(startPosition, endPosition, t / groundedSpeed);
                Vector3 newPosition = transform.position + transform.forward * deltaForward;

                transform.position = newPosition;

                yield return null;
            }
            //transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        animator.SetBool("Walk", false);
        targetPos = transform.position;
        commandManager.NextCommand();
    }

    public override IEnumerator BackwardMove(int index = 1)
    {
        for (int i = 0; i < index; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = startPosition - transform.forward;

            float t = 0.0f;
            while (t < groundedSpeed)
            {
                t += Time.deltaTime;
                Debug.Log((int)transform.forward.z);

                // Calculate which axis is affected by transform.forward
                float tempValue, deltaForward = 0;
                if ((int)transform.forward.x != 0)
                {
                    tempValue = Mathf.Lerp(startPosition.x, endPosition.x, t / groundedSpeed);
                    deltaForward = Mathf.Abs(tempValue - transform.position.x);
                }
                else if ((int)transform.forward.z != 0)
                {
                    tempValue = Mathf.Lerp(startPosition.z, endPosition.z, t / groundedSpeed);
                    deltaForward = Mathf.Abs(tempValue - transform.position.z);
                    Debug.Log("toward z");
                }

                //Vector3 tempPos = Vector3.Lerp(startPosition, endPosition, t / groundedSpeed);
                Vector3 newPosition = transform.position - transform.forward * deltaForward;

                transform.position = newPosition;

                yield return null;
            }
            //transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        targetPos = transform.position;
        commandManager.NextCommand();
    }

    public override IEnumerator RotateLeft(int index = 1)
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

    public override IEnumerator RotateRight(int index = 1)
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

    public override IEnumerator Jump(float distance = 1f)
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

        boxCollider.enabled = false;
        if (!animator)
            animator = GameObject.FindGameObjectWithTag("Skin").GetComponent<Animator>();

        animator.SetBool("Jump", true);

        // Character on the peak
        while (rb.velocity.y > 0)
        {
            yield return null;
        }
        
        boxCollider.enabled = true;

        // Wait for the character to touch the ground
        while (!isGrounded)
        {
            yield return null;
        }

        animator.SetBool("Jump", false);
        commandManager.NextCommand();
    }

    public override void Push(int amount)
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
            StartCoroutine(ForwardMove(amount));
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
        commandManager.NextCommand();
    }

    public override IEnumerator Wait(int duration)
    {
        yield return new WaitForSeconds(duration);
        commandManager.NextCommand();
    }

    public override void CheckGround()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround))
        {
            if(hit.collider.tag != "Enemy")
                isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public override void Press()
    {
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 1f);
        Interactable interactable = hitInfo.transform.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact();
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

    private IEnumerator FetchComponent()
    {
        yield return new WaitForSeconds(0.1f);

        animator = GameObject.FindGameObjectWithTag("Skin").GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }
}
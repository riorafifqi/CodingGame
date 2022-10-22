using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;
    
    bool isMoving;
    RaycastHit hitInfo;

    CommandManager commandManager;

    public float movingSpeed = 5f;

    private void Awake()
    {
        commandManager = GameObject.Find("Game Manager").GetComponent<CommandManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (Mathf.Abs(transform.position.x - targetPos.x) < 0.1f && Mathf.Abs(transform.position.z - targetPos.z) < 0.1f)   // if finished moving
            {
                //Debug.Log("Achieved");
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                isMoving = false;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, movingSpeed * Time.deltaTime);

            return;
        }
    }

    public void Pushed(Vector3 target)
    {
        targetPos = target;
        startPos = transform.position;
        isMoving = true;
    }
}

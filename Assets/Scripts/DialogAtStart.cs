using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAtStart : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<DialogueTrigger>().TriggerDialogue();
    }
}

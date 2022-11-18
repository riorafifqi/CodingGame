using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        Debug.Log("Triggered");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if(FindObjectOfType<PlayableDirector>() != null)
            FindObjectOfType<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(0);
    }
}

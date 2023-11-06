using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField] bool isTriggerOnStart;

    private void Start()
    {
        if (isTriggerOnStart)
            TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        Debug.Log("Triggered");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if(FindObjectOfType<PlayableDirector>() != null)
            FindObjectOfType<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(0);
    }
}

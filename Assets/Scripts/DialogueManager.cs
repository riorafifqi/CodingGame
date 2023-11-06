using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences;
    Animator dialogueAnimator;

    public TMP_Text dialogueText;

    private void Awake()
    {
        Movement.OnAnyPlayerSpawned += Movement_OnAnyPlayerSpawned;
        sentences = new Queue<string>();
    }

    private void Movement_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        dialogueText = GameObject.Find("DialogueBox").GetComponentInChildren<TMP_Text>();
        dialogueAnimator = GameObject.Find("DialogueBox").GetComponent<Animator>();
        GameObject.Find("DialogButton").GetComponent<Button>().onClick.AddListener(delegate { DisplayNextDialogue(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DisplayNextDialogue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueAnimator.SetBool("IsOpen", true);
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.hard_click));
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string tempSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(tempSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        bool isTags = false;

        foreach (char letter in sentence.ToCharArray())
        {
            if (letter == '<' || isTags)
            {
                isTags = true;
                dialogueText.text += letter;
                if (letter == '>')
                    isTags = false;
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.soft_click));
                dialogueText.text += letter;
                yield return null;
            }
        }
    }

    public void EndDialogue()
    {
        if (FindObjectOfType<PlayableDirector>() != null)
            FindObjectOfType<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(1);
        dialogueAnimator.SetBool("IsOpen", false);
        Debug.Log("End Conversation");
    }
}

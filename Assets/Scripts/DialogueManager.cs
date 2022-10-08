using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences;
    Animator dialogueAnimator;

    public TMP_Text dialogueText;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueAnimator = GameObject.Find("DialogueBox").GetComponent<Animator>();
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
                dialogueText.text += letter;
                yield return null;
            }
        }
    }

    public void EndDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);
        Debug.Log("End Conversation");
    }
}

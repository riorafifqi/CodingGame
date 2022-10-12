using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CommandField : MonoBehaviour
{
    TMP_InputField inputField;
    public TMP_Text lineNumber;

    public Console console;
    public int indexInList;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        console = FindObjectOfType<Console>();
    }

    private void Start()
    {
        inputField.onSubmit.AddListener(delegate { OnSubmitCallback(); });
    }

    // Update is called once per frame
    void Update()
    {
        indexInList = console.commandsPerLine.IndexOf(inputField);
        lineNumber.text = (indexInList + 1).ToString();

        if (inputField.isFocused)
        {
            if (inputField.text == "")
            {
                if (Input.GetKeyDown(KeyCode.Backspace))  // if no text in this commandField
                {
                    if (indexInList == 0)   // don't delete if this is the only commandField
                        return;

                    // delete this commandField and focused to upper command field
                    console.commandsPerLine[indexInList - 1].Select();
                    console.commandsPerLine[indexInList - 1].caretPosition = console.commandsPerLine[indexInList - 1].text.Length;

                    Destroy(this.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (indexInList != console.commandsPerLine.Count - 1)    // if not the last index
                {
                    console.commandsPerLine[indexInList + 1].Select();  // select below line
                    console.commandsPerLine[indexInList + 1].caretPosition = console.commandsPerLine[indexInList + 1].text.Length;  // set caret to the end of code
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (indexInList != 0)    // if not the first index
                {
                    console.commandsPerLine[indexInList - 1].Select();  // select upper line
                    console.commandsPerLine[indexInList - 1].caretPosition = console.commandsPerLine[indexInList - 1].text.Length;  // set caret to the end of code
                }
            }
        }   // end of if else
    }   // end of update

    void OnSubmitCallback()
    {
        // if no more element below, create new
        if (indexInList == console.commandsPerLine.Count - 1)
        {
            GameObject newCommand = Instantiate(console.commandFieldPrefab, console.commandsFieldParent.transform);
            TMP_InputField newCommandInput = newCommand.GetComponentInChildren<TMP_InputField>();

            newCommandInput.Select();
            newCommandInput.caretPosition = newCommandInput.text.Length;
        }
        else
        {
            GameObject newCommand = Instantiate(console.commandFieldPrefab, console.commandsFieldParent.transform);
            TMP_InputField newCommandInput = newCommand.GetComponentInChildren<TMP_InputField>();

            newCommand.transform.SetSiblingIndex(indexInList + 1);

            newCommandInput.Select();
            newCommandInput.caretPosition = newCommandInput.text.Length;
        }
    }
}

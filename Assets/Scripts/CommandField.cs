using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CommandField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public TMP_InputField inputField;
    public TMP_Text lineNumber;
    public GameObject highlight;
    public GameObject inserterLine;

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

    public void AddNewCommand(string command)
    {
        if (inputField.text == "")  // if no text in that line
        {
            inputField.text = command;
            inputField.Select();
            inputField.caretPosition = inputField.text.Length;
        }
        else
        {
            // Instantiate new commandField object
            GameObject newCommand = Instantiate(console.commandFieldPrefab, console.commandsFieldParent.transform);
            TMP_InputField newCommandInput = newCommand.GetComponentInChildren<TMP_InputField>();

            // Set sibling as its next
            newCommand.transform.SetSiblingIndex(indexInList + 1);

            // set newly instantiate object text as command parameter
            newCommandInput.text = command;

            // Select and put caret at the end of line
            newCommandInput.Select();
            newCommandInput.caretPosition = newCommandInput.text.Length;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            TMP_Text draggedText = eventData.pointerDrag.GetComponentInChildren<TMP_Text>();
            AddNewCommand(draggedText.text);
            
            highlight.SetActive(false);
            inserterLine.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)      // if user currently dragging something
        {
            highlight.SetActive(false);
            inserterLine.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)      // if user currently dragging something
        {
            if (inputField.text == "")
                highlight.SetActive(true);
            else
                inserterLine.SetActive(true);
        }
    }
}

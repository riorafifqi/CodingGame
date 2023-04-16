using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public TMP_InputField inputField;
    public TMP_Text lineNumber;
    public GameObject highlight;
    public GameObject inserterLine;

    Suggestion suggestion;
    Console console;
    public int indexInList;

    // Flag etc
    private bool isDeleting = true;

    private void Awake()
    {
        suggestion = GetComponent<Suggestion>();
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
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (isDeleting)
                {
                    if (indexInList == 0)   // don't delete if this is the only commandField
                        return;

                    DeleteInputField();
                }
                else
                {
                    if (inputField.text.Length == 0)  // if no text in this commandField
                    {
                        // Check if input field is already empty
                        if (isDeleting)
                        {
                            DeleteInputField();
                        }
                        else
                            isDeleting = true;  // Set isDeleting flag to true
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!suggestion.IsSuggestionActive())   // if intelisense isn't active
                {
                    if (indexInList != console.commandsPerLine.Count - 1)    // if not the last index
                    {
                        console.commandsPerLine[indexInList + 1].Select();  // select below line
                        console.commandsPerLine[indexInList + 1].caretPosition = console.commandsPerLine[indexInList + 1].text.Length;  // set caret to the end of code
                    }
                }
                else   // if active
                    suggestion.SelectDown();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!suggestion.IsSuggestionActive())   // if intelisense isn't active
                {
                    if (indexInList != 0)    // if not the first index
                    {
                        console.commandsPerLine[indexInList - 1].Select();  // select upper line
                        console.commandsPerLine[indexInList - 1].caretPosition = console.commandsPerLine[indexInList - 1].text.Length;  // set caret to the end of code
                    }
                }
                else   // if active
                    suggestion.SelectUp();
            }
        }   // end of if else
    }   // end of update

    void OnSubmitCallback() // When enter pressed
    {
        SoundManager.Instance.PlaySoundRandomPitch(SoundManager.Instance._Database.GetClip(SFX.hard_click));
        if (!suggestion.IsSuggestionActive())   // if intelisense isn't active
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
        else
        {
            Debug.Log("Applying suggestion");
            suggestion.ApplySuggestion();
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

    public void OnTyping()
    {
        Debug.Log("Currentyly Typing");
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SoundManager.Instance.PlaySoundRandomPitch(SoundManager.Instance._Database.GetClip(SFX.hard_click));
        }
        else
        {
            // Set isDeleting flag to true
            isDeleting = false;
            SoundManager.Instance.PlaySoundRandomPitch(SoundManager.Instance._Database.GetClip(SFX.soft_click));
        }
    }

    private void DeleteInputField()
    {
        // Destroy this command field field
        Destroy(this.gameObject);

        // focused to upper command field
        console.commandsPerLine[indexInList - 1].Select();
        console.commandsPerLine[indexInList - 1].caretPosition = console.commandsPerLine[indexInList - 1].text.Length;
    }
}

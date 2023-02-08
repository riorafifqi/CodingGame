using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Suggestion : MonoBehaviour
{
    CommandField commandField;
    GameManager gameManager;

    public GameObject suggestField;
    public List<string> suggestions;

    public List<GameObject> suggestionButtons;
    public GameObject suggestionButtonParent;

    public int activeSuggestion = 0;
    public int selectedButton;

    private void Awake()
    {
        commandField = GetComponent<CommandField>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManagerMultiplayer>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        suggestions.Clear();
        if (commandField.inputField.isFocused)
        {
            if (commandField.inputField.text != "")
                Check(commandField.inputField.text);
        }
        ShowSuggestion();

        activeSuggestion = suggestions.Count;
        if (activeSuggestion == 0)
            selectedButton = 0;

        Select(selectedButton);
    }

    public void Check(string input)
    {
        foreach (string command in gameManager.legalCommands)
        {
            if (command.StartsWith(input))
            {
                // Add that command to this suggestion field
                suggestions.Add(command);
            }
            
            if (command == input)
            {
                suggestions.Clear();
            }
        }
    }

    public void ShowSuggestion()
    {
        // Deactivate all button
        foreach (GameObject button in suggestionButtons)
        {
            button.SetActive(false);
            activeSuggestion = 0;
        }

        // Get 1 button and turn that button text to suggestion one
        for (int i = 0; i < suggestions.Count; i++)
        {
            // get button text
            suggestionButtons[i].SetActive(true);
            TMP_Text buttonText = suggestionButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = suggestions[i];
        }
    }

    public void SelectUp()
    {
        if (selectedButton == 0)
            return;

        selectedButton--;
    }

    public void SelectDown()
    {
        if (selectedButton == activeSuggestion - 1)
            return;

        selectedButton++;
    }

    void Select(int index)
    {
        // change all buttons transparency to 0
        foreach (GameObject suggestion in suggestionButtons)
        {
            suggestion.GetComponent<Image>().enabled = false;
        }

        suggestionButtons[index].GetComponent<Image>().enabled = true;
    }

    public void ApplySuggestion()
    {
        // get selected button text
        TMP_Text selectedText = suggestionButtons[selectedButton].GetComponentInChildren<TMP_Text>();

        commandField.inputField.text = "";
        commandField.inputField.text = selectedText.text;

        commandField.inputField.ActivateInputField();
        commandField.inputField.Select();

        commandField.inputField.caretPosition = commandField.inputField.text.Length;
        Debug.Log(commandField.inputField.caretPosition);
    }

    public bool IsSuggestionActive()
    {
        if (activeSuggestion == 0)
            return false;
        else
            return true;
    }
}

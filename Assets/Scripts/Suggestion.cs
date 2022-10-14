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


    private void Awake()
    {
        commandField = GetComponent<CommandField>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        suggestions.Clear();
        if (commandField.inputField.isFocused)
        {
            if (commandField.inputField.text != "")
                Check(commandField.inputField.text);

            ShowSuggestion();
        }
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
        }
    }

    public void ShowSuggestion()
    {
        // Deactivate all button
        foreach (GameObject button in suggestionButtons)
        {
            button.SetActive(false);
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
}

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

    public GameObject suggestionButtonPrefab;
    public GameObject suggestionButtonParent;


    private void Awake()
    {
        commandField = GetComponent<CommandField>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        suggestField.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        suggestions.Clear();
        if (commandField.inputField.isFocused && commandField.inputField.text != "")
        {
            Check(commandField.inputField.text);

            if (suggestions.Count != 0)
            {
                suggestField.SetActive(true);
            }
            else if (suggestions.Count == 0)
            {
                suggestField.SetActive(false);
            }
        }
        ShowSuggestion();
    }

    public void Check(string input)
    {
        foreach (string command in gameManager.legalCommands)
        {
            if (command.Contains(input))
            {
                // Add that command to this suggestion field
                suggestions.Add(command);
            }
        }
    }

    public void ShowSuggestion()
    {
        foreach (string suggestion in suggestions)
        {
            GameObject suggestionGo = Instantiate(suggestionButtonPrefab, suggestionButtonParent.transform);
            TMP_Text suggestionText = suggestionGo.GetComponentInChildren<TMP_Text>();

            suggestionText.text = suggestion;
        }
    }
}

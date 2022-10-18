using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    public List<string> validCommand;
    CommandField commandField;
    public string errorText;

    public bool isError;

    private void Start()
    {
        commandField = GetComponent<CommandField>();
    }

    private void Update()
    {
        if (CommandError(commandField.inputField.text))
        {
            // Command error
            errorText = "Command Error";
            isError = true;
        }
        else if (ParenthesesError(commandField.inputField.text))
        {
            // Parenthesis Error
            errorText = "Parentheses Error";
            isError = true;
        }
        else
        {
            errorText = "";
            isError = false;
        }
    }

    private void OnMouseOver()
    {
        // check if there is text true
    }

    public bool CommandError(string input)
    {
        Debug.Log("Command Error");
        foreach (string valid in validCommand)
        {
            if (input.Contains(valid) == false)
            {
                return true;
            }
        }

        return false;
    }

    public bool ParenthesesError(string input)
    {
        Debug.Log("Parentheses Error");
        if (input.Contains('(') == false && input.Contains(')') == false)
        {
            return true;
        }

        return false;
    }
}

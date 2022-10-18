using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    public List<string> validCommand;
    CommandField commandField;
    public string errorText;

    public static bool isError;

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
        foreach (string valid in validCommand)
        {
            if (!input.Contains(valid))
            {
                return true;
            }
        }

        return false;
    }

    public bool ParenthesesError(string input)
    {
        if (!input.Contains('(') && !input.Contains(')'))
        {
            return true;
        }

        return false;
    }
}

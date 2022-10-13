using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Console : MonoBehaviour
{
    public GameObject commandsFieldParent;
    public GameObject commandFieldPrefab;

    [SerializeField] TMP_InputField inputField;
    public int lineCount;
    public List<TMP_InputField> commandsPerLine;
    public string[] runningCommand;
    TextGenerator text;

    public string commandClass;
    public string commandMethod;
    public string commandParams;

    public bool isFinish;

    private void Update()
    {
        LineCount();
        SeparateByLine();
    }

    public void AssignCommand(int index)
    {
        if(index > commandsPerLine.Count)
        {
            isFinish = true;
            Debug.Log("Command Stopped");
            return;
        }

        string legalChars = "1234567890";
        commandParams = "";
        runningCommand = commandsPerLine[index].text.Split(char.Parse("."));
        
        commandClass = runningCommand[0];
        if(runningCommand.Length > 1)
            commandMethod = runningCommand[1];

        foreach (var number in commandsPerLine[index].text)    // Extract number
        {
            if(legalChars.Contains(number))
            {
                commandParams += number;
            }
        }

        HighlightCommand(index);
    }

    public void SeparateByLine()
    {
        commandsPerLine.Clear();
        // commandsPerLine = inputField.text.Split(char.Parse("\n"));
        foreach (Transform child in commandsFieldParent.transform)
        {
            commandsPerLine.Add(child.GetComponentInChildren<TMP_InputField>());
        }
    }

    void LineCount()
    {
        lineCount = 1;
        foreach (var letter in inputField.text)
        {
            if (letter == '\n')
            {
                lineCount++;
            }
        }
    }

    void HighlightCommand(int index)
    {
        foreach (var command in commandsPerLine)
        {
            CommandField comField = command.GetComponentInParent<CommandField>();
            comField.highlight.SetActive(false);
        }
        commandsPerLine[index].GetComponentInParent<CommandField>().highlight.SetActive(true);

    }

}

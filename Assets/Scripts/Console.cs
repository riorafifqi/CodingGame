using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    public int lineCount;
    public string[] commandsPerLine;
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
        Debug.Log(CheckEmptyLine());
    }

    public void AssignCommand(int index)
    {
        if(index > commandsPerLine.Length)
        {
            isFinish = true;
            Debug.Log("Command Stopped");
            return;
        }

        string legalChars = "1234567890";
        commandParams = "";
        runningCommand = commandsPerLine[index].Split(char.Parse("."));
        
        commandClass = runningCommand[0];
        if(runningCommand.Length > 1)
            commandMethod = runningCommand[1];

        foreach (var number in commandsPerLine[index])    // Extract number
        {
            if(legalChars.Contains(number))
            {
                commandParams += number;
            }
        }
    }

    public void SeparateByLine()
    {
        commandsPerLine = inputField.text.Split(char.Parse("\n"));
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

    bool CheckEmptyLine()
    {
        foreach (string line in commandsPerLine)
        {
            if (line == "")
            {
                return true;
            }
        }

        return false;
    }

    public void AddNewCommand(string command)
    {
        string[] tempString = commandsPerLine;

        if (CheckEmptyLine())
        {
            for (int i = 0; i < lineCount; i++)
            {
                if (tempString[i] == "")
                {
                    tempString[i] = command;
                    inputField.text = "";
                    for (int j = 0; j < tempString.Length; j++)
                    {
                        if (j == tempString.Length - 1)
                        {
                            inputField.text += tempString[j];
                        } else
                            inputField.text += tempString[j] + "\n";

                    }
                    return;
                }
            }
        }
        else
        {
            inputField.text += "\n" + command;
            return;
        }
    }

    public void Test()
    {
        string test = "test";

        AddNewCommand(test);
    }
}

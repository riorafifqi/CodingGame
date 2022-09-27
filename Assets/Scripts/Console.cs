using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    public string[] commandsPerLine;
    public string[] runningCommand;

    public string commandClass;
    public string commandMethod;
    public string commandParams;

    public bool isFinish;

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
}

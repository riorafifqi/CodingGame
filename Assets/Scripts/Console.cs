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


    // Update is called once per frame
    void Update()
    {
    }

    public void Separate()
    {
        string legalChars = "1234567890";
        commandParams = "";

        commandsPerLine = inputField.text.Split(char.Parse("\n"));
        runningCommand = commandsPerLine[0].Split(char.Parse("."));
        
        commandClass = runningCommand[0];
        if(runningCommand.Length > 1)
            commandMethod = runningCommand[1];

        foreach (var letter in commandMethod)    // Extract number
        {
            if(legalChars.Contains(letter))
            {
                commandParams += letter;
            }
        }
    }
}

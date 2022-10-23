using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Console : MonoBehaviour
{
    public GameObject commandsFieldParent;
    public GameObject commandFieldPrefab;
    public GameObject helpPanel;

    public int lineCount;
    public TMP_Text lineText;

    public List<TMP_InputField> commandsPerLine;
    public string[] runningCommand;
    TextGenerator text;

    public string commandClass;
    public string commandMethod;
    public string commandParams;

    public bool isFinish;

    [SerializeField] Color first;
    [SerializeField] Color number;

    private void Start()
    {
        isFinish = false;
    }

    private void Update()
    {
        LineCount();
        SeparateByLine();
        if (isFinish == true)
        {
            // turn off all highlight
            foreach (var command in commandsPerLine)
            {
                CommandField comField = command.GetComponentInParent<CommandField>();
                comField.highlight.SetActive(false);
            }
        }
    }

    public void AssignCommand(int index)
    {
        if (isFinish)
        {
            Debug.Log("Command Stopped");
            return;
        }

        string legalChars = "1234567890";
        commandParams = "";
        runningCommand = commandsPerLine[index].text.Split(char.Parse("."));

        commandClass = runningCommand[0];
        if (runningCommand.Length > 1)
            commandMethod = runningCommand[1];

        foreach (var number in commandsPerLine[index].text)    // Extract number
        {
            if (legalChars.Contains(number))
            {
                commandParams += number;
            }
        }

        HighlightCommand(index);
    }

    public void SeparateByLine()
    {
        commandsPerLine.Clear();
        //commandsPerLine = inputField.text.Split(char.Parse("\n"));
        foreach (Transform child in commandsFieldParent.transform)
        {
            commandsPerLine.Add(child.GetComponentInChildren<TMP_InputField>());
        }
    }

    void LineCount()
    {
        lineCount = 0;
        foreach (Transform child in commandsFieldParent.transform)
        {
            lineCount++;
        }
        lineText.text = "Line : " + lineCount;
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

    public void HelpPanelToggle()
    {
        //helpPanel.transform.position = new Vector3(0, 0, 0);
        if (helpPanel.activeSelf)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.open_menu));
            helpPanel.SetActive(false);
        }
        else if (!helpPanel.activeSelf)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.close));
            helpPanel.SetActive(true);
        }
    }

    string ReplaceStringColor(string source, string find, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);

        //int wordIndex = source.IndexOf(find);

        return hexColor;
    }

    string ReplaceStringColor(string source, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return "<color =\"" + hexColor + "\">" + hexColor + "</color>";
    }
}

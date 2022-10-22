using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Movement movement;
    public Console console;
    
    public Stopwatch stopwatch;


    public int currentCommandIndex;

    private void Awake()
    {
        stopwatch = GetComponent<Stopwatch>();
        movement = GameObject.Find("Player").GetComponent<Movement>();
    }

    private void Update()
    {
        if (console.isFinish)
        {
            stopwatch.StopStopwatch();
        }
    }

    public void OnPressRunCommand()     // On first time running command
    {
        StopAllCoroutines();

        stopwatch.ResetStopwatch();
        stopwatch.StartStopwatch();

        movement.ResetPosition();
        currentCommandIndex = 0;
        console.isFinish = false;

        console.SeparateByLine();
        console.AssignCommand(currentCommandIndex);
        StartCoroutine(RunCommand());

        
    }

    public IEnumerator RunCommand()
    {
        Debug.Log("Run Command");
        //console.Separate();
        yield return new WaitForSeconds(0.5f);

        if (console.commandClass.Contains('('))
        {
            int index = console.commandClass.IndexOf('(');
            console.commandClass = console.commandClass.Remove(index);
            Debug.Log(console.commandClass);
        }

        switch (console.commandClass)
        {
            case "Move":
                if (console.commandMethod.Contains("Forward"))
                {
                    if(console.commandParams != "")
                        movement.MoveForward(int.Parse(console.commandParams));
                    else
                        movement.MoveForward(1);
                }
                else if (console.commandMethod.Contains("Backward"))
                {
                    if (console.commandParams != "")
                        movement.MoveBackward(int.Parse(console.commandParams));
                    else
                        movement.MoveBackward(1);
                }
                else
                {
                    Debug.LogError("Invalid Method");
                }
                break;
            case "Rotate":
                if (console.commandMethod.Contains("Right"))
                {
                    StopAllCoroutines();
                    if (console.commandParams != "")
                        StartCoroutine(movement.RotateRight(int.Parse(console.commandParams)));
                    else
                        StartCoroutine(movement.RotateRight(1));
                }
                else if (console.commandMethod.Contains("Left"))
                {
                    StopAllCoroutines();
                    if (console.commandParams != "")
                        StartCoroutine(movement.RotateLeft(int.Parse(console.commandParams)));
                    else
                        StartCoroutine(movement.RotateLeft(1));
                }
                break;
            case "Jump":
                movement.Jump();
                break;
            case "Interact":
                if (console.commandMethod.Contains("Push"))
                {
                    movement.Push(int.Parse(console.commandParams));
                }
                else if (console.commandMethod.Contains("Press()"))
                {
                    movement.Press();
                }
                break;
            case "":
                movement.Empty();
                break;
            case "Wait":
                if (console.commandParams != "")
                    StartCoroutine(movement.Wait(int.Parse(console.commandParams)));
                else
                    StartCoroutine(movement.Wait(0));
                break;
            default:
                Debug.Log("Command Error");
                break;
        }
    }

    public void NextCommand()
    {
        if (currentCommandIndex == console.lineCount - 1)
        {
            console.isFinish = true;
            return;
        }

        currentCommandIndex++;
        console.AssignCommand(currentCommandIndex);
        StartCoroutine(RunCommand());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Movement movement;
    public Console console;
    public int currentCommandIndex;

    private void Awake()
    {
        movement = GameObject.Find("Player").GetComponent<Movement>();
    }

    public void OnPressRunCommand()     // On first time running command
    {
        movement.ResetPosition();
        currentCommandIndex = 0;
        //console.isFinish = false;

        console.SeparateByLine();
        console.AssignCommand(currentCommandIndex);
        StartCoroutine(RunCommand());


        /*do
        {
            console.AssignCommand(currentCommandIndex);
            RunCommand();
        } while (currentCommandIndex <= console.commandsPerLine.Length);*/

    }

    public IEnumerator RunCommand()
    {
        Debug.Log("Run Command");
        //console.Separate();
        yield return new WaitForSeconds(0.5f);

        switch (console.commandClass)
        {
            case "Move":
                if (console.commandMethod.Contains("Forward"))
                {
                    movement.MoveForward(int.Parse(console.commandParams));
                }
                else if (console.commandMethod.Contains("Backward"))
                {
                    movement.MoveBackward(int.Parse(console.commandParams));
                }
                else
                {
                    Debug.LogError("Invalid Method");
                }
                break;
            case "Rotate":
                if (console.commandMethod.Contains("Right()"))
                {
                    movement.RotateRight();
                }
                else if (console.commandMethod.Contains("Left()"))
                {
                    movement.RotateLeft();
                }
                break;
            case "Jump()":
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
            default:
                Debug.Log("Command Error");
                break;
        }
    }

    public void NextCommand()
    {
        /*if (!console.isFinish)
        {
            
        }*/
        currentCommandIndex++;
        console.AssignCommand(currentCommandIndex);
        StartCoroutine(RunCommand());
    }

    IEnumerator Delay(float value)
    {
        Debug.Log("Called");
        yield return new WaitForSeconds(value);
    }
}

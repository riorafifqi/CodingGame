using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Movement movement;
    public Console console;
    public int currentCommandIndex;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressRunCommand()     // On first time running command
    {
        movement.ResetPosition();
        currentCommandIndex = 0;

        console.SeparateByLine();
        console.AssignCommand(currentCommandIndex);
        RunCommand();


        /*do
        {
            console.AssignCommand(currentCommandIndex);
            RunCommand();
        } while (currentCommandIndex <= console.commandsPerLine.Length);*/

    }

    public void RunCommand()
    {
        //console.Separate();

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
                if (console.commandMethod.Contains("Right"))
                {
                    movement.RotateRight();
                }
                else if (console.commandMethod.Contains("Left"))
                {
                    movement.RotateLeft();
                }
                break;
            case "Jump":
                movement.Jump();
                break;
            default:
                Debug.Log("Command Error");
                break;

        }
    }

    public void NextCommand()
    {
        currentCommandIndex++;
        console.AssignCommand(currentCommandIndex);
        RunCommand();
    }
}

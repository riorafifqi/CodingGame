using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Movement movement;
    public Console console;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunCommand()
    {
        console.Separate();

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
}

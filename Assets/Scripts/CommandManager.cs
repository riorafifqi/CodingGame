using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public Movement movement;
    public Console console;
    public List<Vector3> spawnPos;

    public Stopwatch stopwatch;
    GameManager gameManager;

    public int currentCommandIndex;

    private void OnEnable()
    {
        GameplayEvent.OnTimeIsUpE += OnPressRunCommand;
    }

    private void OnDisable()
    {
        GameplayEvent.OnTimeIsUpE -= OnPressRunCommand;
    }

    private void Awake()
    {
        if (Movement.LocalInstance != null)
            movement = Movement.LocalInstance;
        else
            Movement.OnAnyPlayerSpawned += Movement_OnAnyPlayerSpawned;

        stopwatch = GetComponent<Stopwatch>();
        gameManager = GetComponent<GameManager>();
        console = FindObjectOfType<Console>();
    }

    private void Movement_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (Movement.LocalInstance != null)
            movement = Movement.LocalInstance;
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.level));
    }

    private void Update()
    {
        if (console.isFinish)
        {
            stopwatch.StopStopwatch();
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            OnPressRunCommand();
        }
    }

    public virtual void OnPressRunCommand()     // On first time running command
    {
        StartCoroutine(OnPressRunCommandCoroutine());
    }

    private IEnumerator OnPressRunCommandCoroutine()
    {
        GameplayEvent.OnStartRunning();

        if (MultiplayerFlowManager.playMultiplayer)
            yield return new WaitForSeconds(2f);
        else
            yield return null;

        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(SFX.confirm));
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.go));

        StopAllCoroutines();

        stopwatch.ResetStopwatch();
        stopwatch.StartStopwatch();

        gameManager.ResetLevel();

        currentCommandIndex = 0;
        console.isFinish = false;

        console.SeparateByLine();
        console.AssignCommand(currentCommandIndex);
        StartCoroutine(RunCommand());
    }

    public virtual IEnumerator RunCommand()
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
                    if (console.commandParams != "")
                        StartCoroutine(movement.ForwardMove(int.Parse(console.commandParams)));
                    else
                        StartCoroutine(movement.ForwardMove(1));
                }
                else if (console.commandMethod.Contains("Backward"))
                {
                    if (console.commandParams != "")
                        StartCoroutine(movement.BackwardMove(int.Parse(console.commandParams)));
                    else
                        StartCoroutine(movement.BackwardMove(1));
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
                StartCoroutine(movement.Jump());
                break;
            case "Interact":
                if (console.commandMethod.Contains("Push"))
                {
                    if (console.commandParams != "")
                        movement.Push(int.Parse(console.commandParams));
                    else
                        movement.Push(1);
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

    public virtual void NextCommand()
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

    public Vector3 GetSpawnPos(int index)
    {
        return spawnPos[index];
    }

    [ClientRpc]
    public void SetAllCommandExecuted_PlayerClientRpc(Movement player, bool status)
    {
        player.SetAllCommandExecuted(status);
    }
}
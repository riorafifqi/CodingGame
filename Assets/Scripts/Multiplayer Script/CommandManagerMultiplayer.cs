using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Procedural;

public class CommandManagerMultiplayer : CommandManager
{
    public new Movement movement;   // our player movement
    public new Movement movementOther;  // second player movement
    public new Console console;

    public new Stopwatch stopwatch;
    GameManagerMultiplayer gameManager;

    public new int currentCommandIndex;

    [HideInInspector] public PhotonView view;

    public Camera selfCamera;
    public Camera otherCamera;
    public GameObject consolePanel;

    private void Awake()
    {
        stopwatch = GetComponent<Stopwatch>();
        gameManager = GetComponent<GameManagerMultiplayer>();
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.level));

        StartCoroutine(CheckAllPlayerConnected());
    }

    private void Update()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length);
        if (console.isFinish)
        {
            stopwatch.StopStopwatch();
        }
    }

    public override void OnPressRunCommand()     // On first time running command
    {
        Debug.Log("Running");

        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.confirm));

        StopAllCoroutines();

        //StartCoroutine(CameraChange());
        CameraSplit();

        stopwatch.ResetStopwatch();
        stopwatch.StartStopwatch();

        currentCommandIndex = 0;
        console.isFinish = false;

        console.SeparateByLine();
        console.AssignCommand(currentCommandIndex);

        if(view.IsMine)
            StartCoroutine(RunCommand());
    }

    public override IEnumerator RunCommand()
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

    public override void NextCommand()
    {
        if (currentCommandIndex == console.lineCount - 1)
        {
            console.isFinish = true;
            return;
        }

        currentCommandIndex++;
        console.AssignCommand(currentCommandIndex);
        StartCoroutine(this.RunCommand());
    }

    IEnumerator CameraChange()
    {
        float waitTime = 2f;
        float elapsedTime = 0f;

        Vector2 selfCameraChange = new Vector2(-0.51f, 0);
        Vector2 otherCameraChange = new Vector2(0.51f, 0);
        Vector2 consolePanelChange = new Vector2(-Screen.width, 0);

        while (elapsedTime < waitTime)
        {
            selfCamera.rect = new Rect(Vector2.Lerp(selfCamera.rect.position, selfCameraChange, (elapsedTime / waitTime)), selfCamera.rect.size);
            otherCamera.rect = new Rect(Vector2.Lerp(otherCamera.rect.position, otherCameraChange, (elapsedTime / waitTime)), otherCamera.rect.size);
            consolePanel.transform.position = Vector2.Lerp(consolePanel.transform.position, consolePanelChange, (elapsedTime / waitTime));
            elapsedTime = elapsedTime + Time.deltaTime;
            yield return null;
        }

        selfCamera.rect = new Rect(selfCameraChange, Vector2.one);
        otherCamera.rect = new Rect(otherCameraChange, Vector2.one);
        consolePanel.transform.position = consolePanelChange;
        yield return null;

    }

    public void CameraSplit()
    {
        Vector2 selfCameraChange = new Vector2(-0.51f, 0);
        Vector2 otherCameraChange = new Vector2(0.51f, 0);
        Vector2 consolePanelChange = new Vector2(-Screen.width, 0);

        selfCamera.rect = new Rect(selfCameraChange, Vector2.one);
        otherCamera.rect = new Rect(otherCameraChange, Vector2.one);
        consolePanel.transform.position = consolePanelChange;
    }

    IEnumerator CheckAllPlayerConnected()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == 2);

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            view = player.GetComponent<PhotonView>();
            if (view.IsMine)    // if this is our player
            {
                movement = view.gameObject.GetComponent<Movement>();
                Transform selfCameraParent = selfCamera.transform.parent;
                selfCameraParent.transform.localPosition = new Vector3(movement.transform.localPosition.x, 0.5f, movement.transform.localPosition.z);

                CameraControllerMultiplayer selfController = selfCameraParent.GetComponent<CameraControllerMultiplayer>();
                selfController.SetTarget(movement.transform);
            }
            else if (view.IsMine == false)   // if this is enemy player
            {
                movementOther = view.gameObject.GetComponent<Movement>();
                Transform otherCameraParent = otherCamera.transform.parent;
                otherCameraParent.transform.localPosition = new Vector3(movementOther.transform.localPosition.x, 0.5f, movementOther.transform.localPosition.z);

                CameraControllerMultiplayer otherController = otherCameraParent.GetComponent<CameraControllerMultiplayer>();
                otherController.SetTarget(movementOther.transform);
            }
        }
    }
}

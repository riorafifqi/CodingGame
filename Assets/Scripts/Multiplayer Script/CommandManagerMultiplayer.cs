using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Procedural;

public class CommandManagerMultiplayer : CommandManager
{
    public MovementMultiplayer movementMine;
    public MovementMultiplayer movementOther;  // second player movement, first player in CommandManager

    [HideInInspector] public GameManagerMultiplayer gameManagerMultiplayer;

    [HideInInspector] public PhotonView view;
    [HideInInspector] public PhotonView commandManagerView;

    public Camera selfCamera;
    public Camera otherCamera;
    public GameObject consolePanel;

    public bool isStarting = false;

    private void Awake()
    {
        stopwatch = GetComponent<Stopwatch>();
        gameManagerMultiplayer = GetComponent<GameManagerMultiplayer>();
        commandManagerView = GetComponent<PhotonView>();
        isStarting = false;
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
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.confirm));

        /*if(view.IsMine)
            commandManagerView.RPC("StartCommandRPC", RpcTarget.All);*/
        commandManagerView.RPC("TimesRunOut", RpcTarget.All);
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

        if (view.IsMine)
        {
            switch (console.commandClass)
            {
                case "Move":
                    if (console.commandMethod.Contains("Forward"))
                    {
                        Debug.Log("Move Forward");
                        if (console.commandParams != "")
                            movementMine.MoveForward(int.Parse(console.commandParams));
                        else
                            movementMine.MoveForward(1);
                    }
                    else if (console.commandMethod.Contains("Backward"))
                    {
                        Debug.Log("Move Backward");
                        if (console.commandParams != "")
                            movementMine.MoveBackward(int.Parse(console.commandParams));
                        else
                            movementMine.MoveBackward(1);
                    }
                    else
                    {
                        console.isFinish = true;
                        Debug.LogError("Invalid Method");
                    }
                    break;
                case "Rotate":
                    if (console.commandMethod.Contains("Right"))
                    {
                        Debug.Log("Rotating Right");
                        StopAllCoroutines();
                        if (console.commandParams != "")
                            StartCoroutine(movementMine.RotateRight(int.Parse(console.commandParams)));
                        else
                            StartCoroutine(movementMine.RotateRight(1));
                    }
                    else if (console.commandMethod.Contains("Left"))
                    {
                        Debug.Log("Rotating Left");
                        StopAllCoroutines();
                        if (console.commandParams != "")
                            StartCoroutine(movementMine.RotateLeft(int.Parse(console.commandParams)));
                        else
                            StartCoroutine(movementMine.RotateLeft(1));
                    }
                    else
                        console.isFinish = true;
                    break;
                case "Jump":
                    Debug.Log("Is Jumping");
                    movementMine.Jump();
                    break;
                case "Interact":
                    if (console.commandMethod.Contains("Push"))
                    {
                        Debug.Log("Pushing");
                        if (console.commandParams != "")
                            movementMine.view.RPC("PushRPC", RpcTarget.All, int.Parse(console.commandParams));
                        else
                            movementMine.view.RPC("PushRPC", RpcTarget.All, 1);
                    }
                    else if (console.commandMethod.Contains("Press()"))
                    {
                        movementMine.Press();
                    }
                    break;
                case "":
                    movementMine.Empty();
                    break;
                case "Wait":
                    if (console.commandParams != "")
                        StartCoroutine(movementMine.Wait(int.Parse(console.commandParams)));
                    else
                        StartCoroutine(movementMine.Wait(0));
                    break;
                default:
                    console.isFinish = true;
                    Debug.Log("Command Error");
                    break;
            }
        }
    }

    public override void NextCommand()
    {
        if (view.IsMine)
        {
            if (movementMine.currentCommandIndex >= console.lineCount - 1)
            {
                console.isFinish = true;
                return;
            }

            movementMine.currentCommandIndex++;
            console.AssignCommand(movementMine.currentCommandIndex);
            StartCoroutine(this.RunCommand());
        }
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
        
        yield return new WaitForSeconds(2f);

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
                movementMine = view.gameObject.GetComponent<MovementMultiplayer>();
                Transform selfCameraParent = selfCamera.transform.parent;
                selfCameraParent.transform.localPosition = new Vector3(movementMine.transform.localPosition.x, 0.5f, movementMine.transform.localPosition.z);

                CameraControllerMultiplayer selfController = selfCameraParent.GetComponent<CameraControllerMultiplayer>();
                selfController.SetTarget(movementMine.transform);
            }
            else if (view.IsMine == false)   // if this is enemy player
            {
                movementOther = view.gameObject.GetComponent<MovementMultiplayer>();
                Transform otherCameraParent = otherCamera.transform.parent;
                otherCameraParent.transform.localPosition = new Vector3(movementOther.transform.localPosition.x, 0.5f, movementOther.transform.localPosition.z);

                CameraControllerMultiplayer otherController = otherCameraParent.GetComponent<CameraControllerMultiplayer>();
                otherController.SetTarget(movementOther.transform);
            }
        }

        // set photon view back to ours
        view = movementMine.GetComponent<PhotonView>();
        gameManagerMultiplayer.StartTimer();
    }

    [PunRPC]
    public void StartCommandRPC()
    {
        isStarting = true;

        StopAllCoroutines();

        StartCoroutine(CameraChange());
        // CameraSplit();

        stopwatch.ResetStopwatch();
        stopwatch.StartStopwatch();

        currentCommandIndex = 0;
        console.isFinish = false;

        console.SeparateByLine();
        console.AssignCommand(movementMine.currentCommandIndex);

        StartCoroutine(RunCommand());
    }
}

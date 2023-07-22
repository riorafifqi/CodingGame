using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    private bool isGameFinish;
    public bool isVirusGone;
    public List<GameObject> Viruses;
    public List<GameObject> interactables;
    
    int virusCount;
    int virusKilled;

    public List<string> legalCommands;
    [SerializeField] private Transform playerPrefab;

    [HideInInspector] public CommandManager commandManager;
    public Level levelData;

    GameObjectInspector goInspector;

    /*[SerializeField] private Character[] skinDatabase;
    [SerializeField] private GameObject characterModel;*/

    private void OnEnable()
    {
        GameplayEvent.OnEnemyDestroyedE += AddVirusKilledCount;
    }

    private void OnDisable()
    {
        GameplayEvent.OnEnemyDestroyedE -= AddVirusKilledCount;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
    }

    private void Awake()
    {
        //characterModel = FindObjectOfType<PlayerAnimManager>().gameObject;

        goInspector = gameObject.AddComponent<GameObjectInspector>();
        commandManager = GetComponent<CommandManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Viruses.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        interactables.AddRange(GameObject.FindGameObjectsWithTag("Interactable"));

        if(MultiplayerFlowManager.playMultiplayer)
            virusCount = Viruses.Count / 2;
        else
            virusCount = Viruses.Count;
        isGameFinish = false;

    }

    public void SetHighscore()
    {
        if (commandManager.stopwatch.GetTime() <= levelData.scores[0].time || levelData.scores[0].time == 0)
        {
            levelData.scores[0].time = commandManager.stopwatch.GetTime();
        }

        if (commandManager.console.lineCount <= levelData.scores[0].totalLine || levelData.scores[0].totalLine == 0)
        {
            levelData.scores[0].totalLine = commandManager.console.lineCount;
        }
    }
    
    public virtual void CheckVirus()
    {
        foreach (GameObject virus in Viruses)
        {
            Enemy virusScript = virus.GetComponent<Enemy>();
            if (!virusScript.isDead)
            {
                return;
            }
        }
    }

    public void ResetLevel()
    {
        foreach (GameObject virus in Viruses)
        {
            virus.GetComponent<Enemy>().isDead = false;
            virus.SetActive(true);
        }

        commandManager.console.ResetCommand();
        commandManager.currentCommandIndex = 0;

        foreach (GameObject interact in interactables)
        {
            interact.GetComponent<Push>().ResetPosition();
        }

        commandManager.movement.ResetPosition();
        SoundManager.Instance.PlayMusic(SoundManager.Instance._Database.GetClip(BGM.level));

        isGameFinish = false;
    }

    /*public void ChangeSkin()
    {
        GameObject tempChar = new GameObject();
        foreach (Character skin in skinDatabase)
        {
            if (PlayerPrefs.GetInt("SelectedSkin") == skin.ID)
            {
                tempChar = skin.modelPrefab;
            }
        }

        Instantiate(tempChar, characterModel.transform.position, characterModel.transform.rotation, characterModel.transform.parent);
        tempChar.transform.SetAsFirstSibling();
        Destroy(characterModel);
    }*/

    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    void AddVirusKilledCount()
    {
        virusKilled++;
        if (virusKilled >= virusCount)
        {
            Movement.LocalInstance.SetFinishStatus(true);
        }
    }

    public void SetGameFinish(bool status)
    {
        isGameFinish = status;
    }

    public bool GetGameFinish()
    {
        return isGameFinish;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject choosenSkinPrefab;
            if (MultiplayerFlowManager.playMultiplayer)
            {
                PlayerData playerData = MultiplayerFlowManager.Instance.GetPlayerDataFromClientId(clientId);
                choosenSkinPrefab = MultiplayerFlowManager.Instance.GetPlayerSkin(playerData.skinId);
            }
            else
            {
                choosenSkinPrefab = MultiplayerFlowManager.Instance.GetPlayerSkin(PlayerPrefs.GetInt(MultiplayerFlowManager.SKIN_ID_PLAYERPREFS));
            }

            Transform playerTransform = Instantiate(choosenSkinPrefab.transform);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            if (!MultiplayerFlowManager.playMultiplayer)
                break;
        }
    }
}
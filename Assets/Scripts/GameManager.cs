using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isVirusGone;
    public List<GameObject> Viruses;
    public List<GameObject> interactables;

    public List<string> legalCommands;

    [HideInInspector] public CommandManager commandManager;
    public Level levelData;

    GameObjectInspector goInspector;

    [SerializeField] private Character[] skinDatabase;
    [SerializeField] private GameObject characterModel;

    private void Awake()
    {
        characterModel = FindObjectOfType<PlayerAnimManager>().gameObject;

        goInspector = gameObject.AddComponent<GameObjectInspector>();
        ChangeSkin();
        commandManager = GetComponent<CommandManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isVirusGone = false;
        Viruses.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        interactables.AddRange(GameObject.FindGameObjectsWithTag("Interactable"));

    }

    // Update is called once per frame
    void Update()
    {
        CheckVirus();
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

        isVirusGone = true;
    }

    public void ResetLevel()
    {
        isVirusGone = false;
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
    }

    public void ChangeSkin()
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
    }

    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
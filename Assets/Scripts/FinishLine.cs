using UnityEngine;

public class FinishLine : MonoBehaviour
{
    GameManager gameManager;
    public GameObject winPanel;
    Material[] lampMat;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        lampMat = GameObject.Find("BezierCurve.007").GetComponent<Renderer>().materials;
        ChangeMat(lampMat[4], Color.red);
        ChangeMat(lampMat[1], Color.red * 10);
    }

    private void Start()
    {
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (gameManager.isVirusGone)
            {
                Debug.Log("You Win!");
                //winPanel.SetActive(true);
                gameManager.commandManager.stopwatch.StopStopwatch();

                gameManager.SetHighscore();
            }
            else
            {
                Debug.Log("There's still virus left");
            }
        }
    }

    public void activateFinish()
    {
        ChangeMat(lampMat[4], Color.green);
        ChangeMat(lampMat[1], Color.green * 10);
    }

    void ChangeMat(Material mat, Color color)
    {
        mat.SetColor("_EmissionColor", color);
        mat.SetColor("_Color", color);
    }
}

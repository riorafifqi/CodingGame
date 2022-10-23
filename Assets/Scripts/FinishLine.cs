using UnityEngine;

public class FinishLine : MonoBehaviour
{
    GameManager gameManager;
    WinPanelManager winPanel;

    Material[] lampMat;

    private void Awake()
    {
        winPanel = GameObject.FindObjectOfType<WinPanelManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        lampMat = GameObject.Find("BezierCurve.007").GetComponent<Renderer>().materials;
        ChangeMat(lampMat[4], Color.red);
        ChangeMat(lampMat[1], Color.red * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (gameManager.isVirusGone)
            {
                Debug.Log("You Win!");

                winPanel.SetLineCount(gameManager.commandManager.console.lineCount);
                winPanel.SetTime(gameManager.commandManager.stopwatch.GetTime());
                winPanel.OpenWinPanel();

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

using UnityEngine;

public class FinishLine : MonoBehaviour
{
    GameManager gameManager;
    public GameObject winPanel;
    Material[] lampMat;

    private void Awake()
    {
        //gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        lampMat = GameObject.Find("BezierCurve.007").GetComponent<Renderer>().materials;
        Debug.Log(lampMat[4].name);
        lampMat[4].SetColor("_EmissionColor", Color.blue);
        lampMat[4].SetColor("_Color", Color.blue);
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
                winPanel.SetActive(true);
            }
            else
            {
                Debug.Log("There's still virus left");
            }
        }
    }

    public void activateFinish()
    {
        lampMat[1].SetColor("_EmissionColor", Color.green);
    }
}

using UnityEngine;

public class VirusManager : MonoBehaviour
{
    [SerializeField] GameObject boom;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameManager.Viruses.Remove(this.gameObject);
            ExplodeThenDestroy();
            Debug.Log("Destroy enemy");
        }
    }

    void ExplodeThenDestroy()
    {
        Instantiate(boom, transform.parent.position, transform.parent.rotation, transform.parent.parent);
        Destroy(transform.parent.gameObject);
    }
}

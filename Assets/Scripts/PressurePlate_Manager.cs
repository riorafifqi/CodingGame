using UnityEngine;
using System.Collections;

public class PressurePlate_Manager : MonoBehaviour
{
    [SerializeField] Material materialOn;
    [SerializeField] Material materialOff;
    [SerializeField] Material lampOn;
    [SerializeField] Material lampOff;

    public bool isActive = false;
    public GameObject affectedBlock;

    Vector3 startingPos;
    Vector3 finalPos;

    private void Start()
    {
        affectedBlock.transform.position = new Vector3(affectedBlock.transform.position.x, -10f, affectedBlock.transform.position.z);
    }

    private void Update()
    {
        if (isActive)
        {
            this.GetComponent<Renderer>().material = materialOn;
            affectedBlock.GetComponent<Renderer>().material = lampOn;
        }
        else
        {
            this.GetComponent<Renderer>().material = materialOff;
            affectedBlock.GetComponent<Renderer>().material = lampOff;
        }
    }

    void changeMaterial()
    {
        if (isActive)
            this.GetComponent<Renderer>().material = materialOn;
        else
            this.GetComponent<Renderer>().material = materialOff;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);

        isActive = true;

        // block go up
        startingPos = new Vector3(affectedBlock.transform.position.x, -10f, affectedBlock.transform.position.z);
        finalPos = new Vector3(affectedBlock.transform.position.x, -0.43f, affectedBlock.transform.position.z);
        StartCoroutine(SmoothLerp(1));
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;

        // block go down
        startingPos = new Vector3(affectedBlock.transform.position.x, -0.43f, affectedBlock.transform.position.z);
        finalPos = new Vector3(affectedBlock.transform.position.x, -10f, affectedBlock.transform.position.z);
        StartCoroutine(SmoothLerp(1));
    }

    private IEnumerator SmoothLerp(float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            affectedBlock.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

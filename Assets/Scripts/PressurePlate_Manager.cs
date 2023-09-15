using UnityEngine;
using System.Collections;

public class PressurePlate_Manager : MonoBehaviour
{
    [SerializeField] Material materialOn;
    [SerializeField] Material materialOff;

    public bool isActive = false;
    public GameObject affectedBlock;

    [SerializeField] float descentPosY;
    [SerializeField] float ascentPosY;
    private Vector3 descentPos;
    private Vector3 ascentPos;

    private void Start()
    {
        descentPos = ascentPos = affectedBlock.transform.position;
        descentPos.y = descentPosY;
        ascentPos.y = ascentPosY;

        affectedBlock.transform.position = descentPos;
    }

    private void Update()
    {
        if (isActive)
        {
            this.GetComponent<Renderer>().material = materialOn;
        }
        else
        {
            this.GetComponent<Renderer>().material = materialOff;
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
        /*descentPos = new Vector3(affectedBlock.transform.position.x, -10f, affectedBlock.transform.position.z);
        ascentPos = new Vector3(affectedBlock.transform.position.x, -0.43f, affectedBlock.transform.position.z);*/
        StartCoroutine(SmoothLerp(descentPos, ascentPos, 1));
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;

        // block go down
        /*descentPos = new Vector3(affectedBlock.transform.position.x, -0.43f, affectedBlock.transform.position.z);
        ascentPos = new Vector3(affectedBlock.transform.position.x, -10f, affectedBlock.transform.position.z);*/
        StartCoroutine(SmoothLerp(ascentPos, descentPos, 1));
    }

    private IEnumerator SmoothLerp(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            affectedBlock.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

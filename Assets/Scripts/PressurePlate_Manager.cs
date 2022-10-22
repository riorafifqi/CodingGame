using UnityEngine;

public class PressurePlate_Manager : MonoBehaviour
{
    [SerializeField] Material materialOn;
    [SerializeField] Material materialOff;

    public bool isActive = false;
    public Animator affectedBlock;
    
    private void Update()
    {
        if (isActive)
            this.GetComponent<Renderer>().material = materialOn;
        else
            this.GetComponent<Renderer>().material = materialOff;
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
        if (other.transform.tag == "Player")
        {
            isActive = true;
            // Aniamtion block up go go
            affectedBlock.SetBool("", true);
        }
    }

}

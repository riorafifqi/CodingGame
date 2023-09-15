using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] Transform startPoint;
    public float length = 5000f;

    protected bool isActive;    
    
    protected virtual void FetchComponent()
    {
        Debug.Log("Fetch component from Laser.cs");
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        FetchComponent();
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Turn off laser
        if (!isActive)
        {
            lineRenderer.enabled = false;
            return;
        }            

        lineRenderer.SetPosition(0, startPoint.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, hit.point);
            }

            if (hit.transform.tag == "Player")
            {
                Debug.Log("hit");
                hit.transform.GetComponent<Movement>().Death();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.forward * length);
        }
    }    

    protected void SetActiveLineRenderer(bool active)
    {
        lineRenderer.enabled = active;
    }    
}

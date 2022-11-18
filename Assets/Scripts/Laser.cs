using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] Transform startPoint;
    public float length = 5000f;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
}

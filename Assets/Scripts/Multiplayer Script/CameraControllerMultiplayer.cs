using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControllerMultiplayer : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 difference;
    
    private Vector3 resetCamera;
    public float smoothing = 5f;
    Vector3 offset;

    private bool drag = false;
    Transform target;

    private void LateUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        offset = transform.position - this.target.position;
        resetCamera = transform.position;
    }
}

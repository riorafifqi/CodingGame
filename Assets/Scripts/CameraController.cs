using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 difference;
    
    private Vector3 resetCamera;
    public float smoothing = 5f;
    Vector3 offset;

    private bool drag = false;
    [SerializeField] Transform target;

    public float shakeDuration = .5f;
    public AnimationCurve curve;

    private void OnEnable()
    {
        if (!MultiplayerFlowManager.playMultiplayer)
        {
            // Singleplayer
            if (Movement.LocalInstance != null)
                target = Movement.LocalInstance.transform;
            else
                Movement.OnAnyPlayerSpawned += Movement_OnAnyPlayerSpawned;
        }
    }

    private void OnDisable()
    {
        Movement.OnAnyPlayerSpawned -= Movement_OnAnyPlayerSpawned;
    }

    private void Movement_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (Movement.LocalInstance != null)
        {
            target = Movement.LocalInstance.transform;
            offset = transform.position - target.position;
        }
    }

    private void Start()
    {
        resetCamera = transform.position;
    }


    private void LateUpdate()
    {
        if (!target)
            return;

        if (Input.GetMouseButton(0))
        {
            difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition - transform.position));
            if (drag == false && !EventSystem.current.IsPointerOverGameObject())
            {
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            transform.position = origin - difference * 0.5f;
        }

        if (!Input.GetMouseButton(0))
        {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }

    public IEnumerator ShakeCam()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPos;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}

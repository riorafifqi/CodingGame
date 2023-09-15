using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GameObjectInspector : MonoBehaviour
{
    public GameObject descriptionBox;

    Ray ray;
    RaycastHit hit;

    private bool canShowDescription = true;
    private float cooldownDuration = 0.5f;
    private float lastToggleTime;

    private void Awake()
    {
        descriptionBox = GameObject.Find("DescriptionBox");
    }

    private void Start()
    {
        descriptionBox.SetActive(false);
        descriptionBox.transform.position = new Vector3(Screen.width, Screen.height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            descriptionBox.transform.position = Input.mousePosition;
            if (hit.collider.gameObject.GetComponent<GameObjectDescription>() && !EventSystem.current.IsPointerOverGameObject())
            {
                if (canShowDescription)
                {
                    descriptionBox.GetComponentInChildren<TMP_Text>().text = hit.collider.gameObject.GetComponent<GameObjectDescription>().description;
                    canShowDescription = false;
                    lastToggleTime = Time.time;
                }
                
                descriptionBox.gameObject.SetActive(true);
            }
            else
            {
                // Check if the cooldown has elapsed before hiding the descriptionBox
                if (!canShowDescription && Time.time - lastToggleTime >= cooldownDuration)
                {
                    descriptionBox.gameObject.SetActive(false);
                    canShowDescription = true;
                }
            }
        }
    }
}

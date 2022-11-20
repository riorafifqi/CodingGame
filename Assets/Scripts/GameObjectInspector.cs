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
            if (hit.collider.gameObject.GetComponent<GameObjectDescription>() && !EventSystem.current.IsPointerOverGameObject())
            {
                descriptionBox.GetComponentInChildren<TMP_Text>().text = hit.collider.gameObject.GetComponent<GameObjectDescription>().description;
                descriptionBox.transform.position = Input.mousePosition;
                descriptionBox.gameObject.SetActive(true);
            }
            else
                descriptionBox.gameObject.SetActive(false);
        }
    }
}

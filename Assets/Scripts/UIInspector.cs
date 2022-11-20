using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIInspector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_InputField inspectedInputField;
    string textToBeDisplay;
    public GameObject descriptionBox;

    public void Awake()
    {
        descriptionBox = GameObject.Find("DescriptionBox");
    }

    void Start()
    {
        inspectedInputField = GetComponentInChildren<TMP_InputField>();
        textToBeDisplay = "";
    }

    void DisplayDescription()
    {
        if (inspectedInputField.text.Contains("Move.Forward"))
        {
            textToBeDisplay = "To move forward";
        }
        else if (inspectedInputField.text.Contains("Move.Backward"))
        {
            textToBeDisplay = "To move backward";
        }
        else if (inspectedInputField.text.Contains("Rotate.Left"))
        {
            textToBeDisplay = "Makes you spin counterclockwise";
        }
        else if (inspectedInputField.text.Contains("Rotate.Right"))
        {
            textToBeDisplay = "Makes you spin clockwise";
        }
        else if (inspectedInputField.text.Contains("Jump"))
        {
            textToBeDisplay = "To jump";
        }
        else if (inspectedInputField.text.Contains("Interact.Push"))
        {
            textToBeDisplay = "Push object in front of you";
        }
        else if (inspectedInputField.text.Contains("Wait"))
        {
            textToBeDisplay = "Waiting for seconds";
        }
        else
        {
            textToBeDisplay = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayDescription();
        descriptionBox.GetComponentInChildren<TMP_Text>().text = textToBeDisplay;
        descriptionBox.transform.position = Input.mousePosition;
        descriptionBox.gameObject.SetActive(true);
        if (descriptionBox.GetComponentInChildren<TMP_Text>().text == "")
        {
            descriptionBox.SetActive(false);
        }
        Debug.Log(textToBeDisplay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionBox.SetActive(false);
    }
}

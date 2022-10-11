using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject draggedCommand;
    TMP_Text draggedCommandText;
    CanvasGroup draggedCanvasGroup;

    public TMP_Text commandText;

    private void Awake()
    {
        draggedCommand = GameObject.Find("DraggedCommand");
        draggedCommandText = draggedCommand.GetComponentInChildren<TMP_Text>();
        draggedCanvasGroup = draggedCommand.GetComponent<CanvasGroup>();

        commandText = gameObject.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        draggedCommand.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        draggedCommand.gameObject.SetActive(true);
        draggedCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        draggedCommand.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        draggedCommand.gameObject.SetActive(false);
        draggedCanvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        draggedCommandText.text = commandText.text;
    }
}

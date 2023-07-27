using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.confirm));
        draggedCommand.gameObject.SetActive(true);
        draggedCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        //draggedCommand.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        draggedCommand.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.cancel));
        draggedCommand.gameObject.SetActive(false);
        draggedCanvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.confirm));
        draggedCommandText.text = commandText.text;
    }
}

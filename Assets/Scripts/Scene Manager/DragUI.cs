using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler
{
    public RectTransform dragRectTransform;
    public Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void DisablePanel()
    {
        dragRectTransform.gameObject.SetActive(false);
    }
}

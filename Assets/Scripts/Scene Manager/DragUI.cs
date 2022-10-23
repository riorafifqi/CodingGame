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
        SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.close));
        dragRectTransform.gameObject.SetActive(false);
    }
}

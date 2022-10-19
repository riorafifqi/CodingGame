using TMPro;
using UnityEngine;


public class TextColor : MonoBehaviour
{
    TMP_InputField inputField;
    [SerializeField] Color first;
    [SerializeField] Color number;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
    }

    void OnSubmitCallback()
    {

    }

    string ReplaceStringColor(string source, string find, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return hexColor;
    }
}

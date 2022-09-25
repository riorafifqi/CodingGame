using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static string playerUsername;
    [SerializeField] TMP_InputField nameInputField;

    public void OnClickApplyName()
    {
        playerUsername = nameInputField.text;
        nameInputField.text = "";
    }
}

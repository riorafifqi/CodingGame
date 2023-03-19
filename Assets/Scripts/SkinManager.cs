using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinManager : MonoBehaviour
{
    [SerializeField] Button changeButton;
    [SerializeField] GameObject skinCollection;
    [SerializeField] Image iconOnMenu;
    private SkinSelector[] skins;

    private void Start()
    {
        skins = skinCollection.GetComponentsInChildren<SkinSelector>();

        changeButton.onClick.AddListener(OnClickChange);

        foreach (var skin in skins)
        {
            Character tempChar = skin.GetCharacter();
            if (tempChar.ID == PlayerPrefs.GetInt("SelectedSkin"))
            {
                iconOnMenu.sprite = tempChar.icon;
                break;
            }
        }
    }

    private void OnClickChange()
    {
        foreach (SkinSelector skin in skins)
        {
            if (skin.GetSelectedStatus() == true)
            {
                PlayerPrefs.SetInt("SelectedSkin", skin.GetCharacter().ID);
                iconOnMenu.sprite = skin.GetCharacter().icon;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace CypherCode
{
    public class CustomLevelItem : MonoBehaviour
    {
        public TMP_Text levelUIName;
        public string fullLevelName;
        public GameObject selected;

        // Start is called before the first frame update
        void Start()
        {
            string[] parts = fullLevelName.Split('_');

            if(fullLevelName != "")
                levelUIName.text = parts[1];
            IfSelected();
        }

        void IfSelected()
        {
            if (fullLevelName == CustomLevelLoader.selectedLevelFileName)
            {
                selected.SetActive(true);
                levelUIName.text = "<color=#000000>" + levelUIName.text + "</color>";
            }
            else
            {
                selected.SetActive(false);
            }
        }

        public void SelectLevel()
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance._Database.GetClip(SFX.menu_selection));
            CustomLevelLoader.selectedLevelFileName = fullLevelName;
            FindObjectOfType<CustomLevelInspector>().UpdateUI();
            GetComponentInParent<CustomLevelLoader>().UpdateSelected();
        }
    }
}

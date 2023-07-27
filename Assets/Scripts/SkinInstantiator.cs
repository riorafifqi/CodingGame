using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CypherCode
{
    public class SkinInstantiator : MonoBehaviour
    {
        [SerializeField] private Character[] skinDatabase;
        [SerializeField] private GameObject charPlaceholder;

        // Start is called before the first frame update
        private void Awake()
        {
            //ChangeSkin();
        }

        public void ChangeSkin()
        {
            GameObject tempChar = new GameObject();
            foreach (Character skin in skinDatabase)
            {
                if (PlayerPrefs.GetInt("SelectedSkin") == skin.ID)
                {
                    tempChar = skin.modelUIPrefab;
                }
            }

            Instantiate(tempChar, charPlaceholder.transform.position, charPlaceholder.transform.rotation, charPlaceholder.transform.parent);
            Destroy(charPlaceholder);
        }
    }
}

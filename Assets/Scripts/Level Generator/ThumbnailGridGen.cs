using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CypherCode
{
    public class ThumbnailGridGen : MonoBehaviour
    {
        public GameObject thumbnailPrefab;
        public Transform gridParent;
        public GameObject[] prefabsList;
        // Start is called before the first frame update
        void Start()
        {
            ClearGrid();

            foreach(var i in prefabsList)
            {
                GameObject thumbnail = Instantiate(thumbnailPrefab, gridParent);
                ObjectUI objectUI = thumbnail.GetComponent<ObjectUI>();

                if(objectUI != null)
                {
                    objectUI.prefab = i;
                    objectUI.UpdateThumbnail();
                }
                else
                {
                    Debug.LogError("ObjectUI component not found on thumbnail prefab");
                }
            }
        }

        private void ClearGrid()
        {
            // Clear the grid by destroying all child elements
            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

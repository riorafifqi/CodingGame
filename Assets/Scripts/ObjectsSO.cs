using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prefab Data", menuName = "Custom/Prefab Data")]
public class ObjectsSO : ScriptableObject
{
    public GameObject[] prefabs;
    public Texture2D[] thumbnails;

    public Texture2D FindThumbnail(GameObject prefab)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == prefab)
            {
                return thumbnails[i];
            }
        }

        return null;
    }
}

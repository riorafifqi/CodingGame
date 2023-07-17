using JetBrains.Annotations;
using UnityEngine;


    [System.Serializable]
    public struct ColorToStaticPrefab
    {
        public string name;
        public Color color;
        public GameObject prefab;
    }

    [System.Serializable]
    public struct ColorToRotateablePrefab
    {
        public string name;
        public Vector2 color;
        public GameObject prefab;
    }

    [System.Serializable]
    public struct ColorToSpecialPrefab
    {
        public string name;
        public Vector2 color;
        public GameObject prefab;
    }

    [CreateAssetMenu(fileName = "LevelGen", menuName = "LevelEditor/Dictionary")]
    public class ColorToPrefabDictionary : ScriptableObject
    {
        public ColorToStaticPrefab[] colorToPrefabDictionary;
        public ColorToRotateablePrefab[] colorToRotateablePrefabDictionary;
        public ColorToSpecialPrefab[] colorToSpecialPrefabDictionary;
    }

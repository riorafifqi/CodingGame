using System.IO;
using TMPro;
using Unity.Mathematics;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CypherCode
{
    public class LevelGen : MonoBehaviour
    {
        public Texture2D map;
        public ColorToPrefabDictionary colorMapDictionary;
        public Vector3 positionOffset;
        public int numLayers;
        int layerWidth;
        string levelName;

        void Awake()
        {
            if(map == null)
            {
                string folderName = "CypherCodeCustoms";
                string path = Path.Combine(Application.persistentDataPath, folderName);

                if(CustomLevelLoader.selectedLevelFileName != null || CustomLevelLoader.selectedLevelFileName == "")
                {
                    string[] parts = CustomLevelLoader.selectedLevelFileName.Split('_');
                    path += "/" + parts[0] + "_" + parts[1] + "_" + parts[2] + "_" + parts[3];
                    numLayers = int.Parse(parts[2]);
                }
                else
                {
                    path += "/Level_Sampler​_3_Udin.png";
                }
                
                map = LoadImageFromFile(path);
            }
            else
            {
                //numLayers = int.Parse(map.name.Split('_')[2]);
                GetDataFromName();
            }

            colorMapDictionary = Resources.Load<ColorToPrefabDictionary>("LevelGen");
            Debug.Log(map.name);
            layerWidth = map.width / numLayers;
            GenerateLevel();
        }

        private Texture2D LoadImageFromFile(string path)
        {
            // Load the image data into a byte array
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            // Create a new Texture2D and load the image data into it using ImageConversion.LoadImage
            Texture2D texture = new Texture2D(11, 22);
            if (ImageConversion.LoadImage(texture, bytes))
            {
                // Set point filtering on the texture
                texture.filterMode = FilterMode.Point;
                
                
                return texture;
            }
            else
            {
                Debug.LogError("Failed to load image data into the Texture2D.");
                return null;
            }
        }

        void GetDataFromName()
        {
            string[] parts = map.name.Split('_');
            levelName = parts[1];
            numLayers = int.Parse(parts[2]);
            //Debug.Log(map.name + " | Level name : " + levelName + " | Layer : " + numLayers);
        }

        void GenerateLevel()
        {
            //Debug.Log("Generating level : " + levelName);
            for (int x = 0; x < map.width; x++)
            {
                int currentLayer = x / layerWidth;
                for (int y = 0; y < map.height; y++)
                {
                    GenerateTile(x, y, currentLayer);
                }
            }
        }

        void GenerateTile(int x, int y, int layer)
        {
            Color pixelColor = map.GetPixel(x, y);
            if (pixelColor == Color.black) { return; } //if alpha, gtfo

            //Debug.Log("Pixel color at : " + x + " " + y + " is " + pixelColor);
            Vector3 position = new Vector3(x % layerWidth + positionOffset.x, layer + positionOffset.z, y + positionOffset.y);

            foreach (ColorToStaticPrefab cm in colorMapDictionary.colorToPrefabDictionary)
            {
                if (cm.color.Equals(pixelColor))
                {
                    if(cm.name.Equals("Virus"))
                    {
                        position.y -= 0.5f;
                    }
                    Instantiate(cm.prefab, position, quaternion.identity, transform); 
                }
            }

            foreach (ColorToRotateablePrefab cm in colorMapDictionary.colorToRotateablePrefabDictionary)
            {
                Vector2 pixelColorRG = new Vector2(pixelColor.r * 255, pixelColor.g * 255);

                if(cm.color.Equals(pixelColorRG))
                {
                    Quaternion tempRot = cm.prefab.transform.rotation;
                    //float yRot = math.round((255f/360f) * (pixelColor.b * 255));
                    float yRot = math.round(pixelColor.b * 360);
                    //Debug.Log(pixelColor.b * 360f);
                    tempRot = Quaternion.AngleAxis(yRot, Vector3.up);
                    GameObject tempObject = Instantiate(cm.prefab, position, tempRot, transform);
                }
            }
        }
    }
}

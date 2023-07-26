using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.SceneManagement;

namespace CypherCode
{
    public class CaptureLevel : MonoBehaviour
    {
        public GameObject levelRoot; // The root object of the level hierarchy
        public int currentLayer;
        RenderTexture renderTexture;
        public Vector2Int targetSize;
        int numLayers;
        public float levelOffset = -0.5f;
        public int levelSizeOffset = 4;
        public string levelName;
        public TMP_Text levelNameInput;

        private void Awake()
        {
            renderTexture = Resources.Load<RenderTexture>("ColorMapRT");
        }

        private void Start()
        {
            //DisableOtherLayerRenderers(currentLayer);
            //Debug.Log(GetLevelBounds().size);
            targetSize = new Vector2Int((int)GetLevelBounds().size.x, (int)GetLevelBounds().size.z);
            numLayers = 0; //(int)GetLevelBounds().size.y;
        }

        private Bounds GetLevelBounds()
        {
            // Calculate the bounds of the level root object
            Renderer[] renderers = levelRoot.GetComponentsInChildren<Renderer>();
            Bounds levelBounds = new Bounds(levelRoot.transform.position, Vector3.zero);
            Bounds levelBoundsY1 = new Bounds(levelRoot.transform.position, Vector3.zero);
            foreach (Renderer renderer in renderers)
            {
                if (renderer.transform.position.y < 0)
                {
                    if (!renderer.transform.name.Contains("Floor"))
                        continue;
                    levelBoundsY1.Encapsulate(renderer.bounds);
                    //Debug.Log(renderer.transform.name);
                }
                //if (renderer.transform.name.Contains("FinishLine") ||
                //    renderer.transform.name.Contains("BezierCurve.007") ||
                //    renderer.transform.name.Contains("Gear") ||
                //    renderer.transform.name.Contains("Gear.001") ||
                //    renderer.transform.name.Contains("Plane")
                //    )
                //    continue;
                levelBounds.Encapsulate(renderer.bounds);
            }


            float sizeX = (int)levelBoundsY1.size.x + levelSizeOffset;
            float sizeY = (int)levelBounds.size.y;
            float sizeZ = (int)levelBoundsY1.size.z + levelSizeOffset;

            if(levelBoundsY1.size.x % 2 == 0) sizeX -= 1;
            else if(levelBoundsY1.size.z % 2 == 0) sizeZ -= 1;

            if (sizeY < 2)
            {
                levelBounds.size = new Vector3(sizeX, 1f, sizeZ);
            }
            else
            {
                levelBounds.size = new Vector3(sizeX, sizeY, sizeZ);
            }

            return levelBounds;
        }

        private void DisableOtherLayerRenderers(int currentLayer)
        {
            // Disable renderers of objects on higher layers
            Renderer[] renderers = levelRoot.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                float yPos = Mathf.Round(renderer.transform.position.y * 10f) / 10f;
                if (renderer.transform.name == "VirusPlane" && Mathf.Round(renderer.transform.position.y) == currentLayer - 1)
                {
                    renderer.enabled = true;
                }
                else if (yPos != currentLayer + levelOffset)
                {
                    renderer.enabled = false;
                }

            }

            //foreach (Renderer renderer in renderers)
            //{
            //    // Get the parent object's name
            //    string parentName = renderer.transform.parent.gameObject.name;

            //    // Check the parent object's name and assign the appropriate offset
            //    float offset = 0f;

            //    switch (parentName)
            //    {
            //        case "Floors":
            //            offset = -0.5f; // Set the offset for floors
            //            break;
            //        case "Level1":
            //            offset = 0f; // Set the offset for Level1
            //            break;
            //        case "Level2":
            //            offset = 1f; // Set the offset for Level2
            //            break;
            //        case "Level3":
            //            offset = 2f; // Set the offset for Level3
            //            break;
            //        case "Level4":
            //            offset = 3f; // Set the offset for Level4
            //            break;
            //        default:
            //            break;
            //    }

            //    if (renderer.transform.position.y != currentLayer + offset)
            //    {
            //        renderer.enabled = false;
            //    }
            //}
        }

        private void EnableAllRenderers()
        {
            Renderer[] renderers = levelRoot.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
        }

        public void SaveRenderTextureToImage(RenderTexture rt)
        {
            if (rt != null)
            {
                Debug.Log((int)GetLevelBounds().size.y);
                numLayers = (int)GetLevelBounds().size.y + 1;
                int width = rt.width;
                int height = rt.height;

                Texture2D[] layerTextures = new Texture2D[numLayers];

                // Calculate the target width and height
                int targetWidth = targetSize.x * numLayers;
                int targetHeight = targetSize.y;

                // Capture each layer with a delay
                StartCoroutine(CaptureLayersWithDelay(rt, width, height, layerTextures, targetWidth, targetHeight));
            }
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("RevampedMainMenu");
        }

        private IEnumerator CaptureLayersWithDelay(RenderTexture rt, int width, int height, Texture2D[] layerTextures, int targetWidth, int targetHeight)
        {
            for (int layer = 0; layer < numLayers; layer++)
            {
                //Debug.Log("Capturing layer : " + layer + 1);
                DisableOtherLayerRenderers(layer);

                yield return new WaitForEndOfFrame();

                layerTextures[layer] = CaptureLayerTexture(rt, width, height);

                EnableAllRenderers();

                yield return new WaitForSeconds(2.5f);
            }

            // Combine the layer textures into a final texture
            Texture2D finalTexture = CombineLayerTextures(layerTextures, targetWidth, targetHeight);

            // Save the final texture as a PNG file
            byte[] bytes = finalTexture.EncodeToPNG();

            string folderName = "CypherCodeCustoms";
            string path = Path.Combine(Application.persistentDataPath, folderName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (levelName == "" || levelName == null)
                levelName = "CustomLevel";
            
            

            File.WriteAllBytes(path + "/Level_" + levelName + "_" + numLayers + "Username" + ".png", bytes);

            //File.WriteAllBytes("Assets/CustomLevelList/Level_" + levelName + "_" + numLayers + ".png", bytes);

            // Destroy the textures to free up memory
            foreach (Texture2D tex in layerTextures)
            {
                Destroy(tex);
            }
            Destroy(finalTexture);
        }

        private Texture2D CaptureLayerTexture(RenderTexture rt, int width, int height)
        {
            levelName = levelNameInput.text;

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBAHalf, false);
            RenderTexture.active = rt;
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            RenderTexture.active = null;
            texture.filterMode = FilterMode.Point;

            return texture;
        }

        private Texture2D CombineLayerTextures(Texture2D[] layerTextures, int targetWidth, int targetHeight)
        {
            Texture2D finalTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBAHalf, false);

            for (int layer = 0; layer < numLayers; layer++)
            {
                for (int x = 0; x < targetSize.x; x++)
                {
                    int targetX = layer * targetSize.x + x;

                    for (int y = 0; y < targetSize.y; y++)
                    {
                        int sourceX = (int)(x * (float)(layerTextures[layer].width - 1) / (targetSize.x - 1));
                        int sourceY = (int)(y * (float)(layerTextures[layer].height - 1) / (targetSize.y - 1));

                        Color sourceColor = layerTextures[layer].GetPixel(sourceX, sourceY);

                        // Apply gamma correction
                        sourceColor.r = Mathf.LinearToGammaSpace(sourceColor.r);
                        sourceColor.g = Mathf.LinearToGammaSpace(sourceColor.g);
                        sourceColor.b = Mathf.LinearToGammaSpace(sourceColor.b);

                        finalTexture.SetPixel(targetX, y, sourceColor);
                    }
                }
            }

            finalTexture.Apply();
            finalTexture.filterMode = FilterMode.Point;

            return finalTexture;
        }
    }
}

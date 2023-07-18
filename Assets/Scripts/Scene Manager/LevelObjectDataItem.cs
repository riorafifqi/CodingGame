using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelObjectDataItem : MonoBehaviour
{
    public TMP_Text text;
    public Image icon;

    public GameObject prefab;

    public void UpdateText(int count)
    {
        text.text = count + "X " + AddSpacesToPrefabName(prefab.name);
    }

    public void UpdateThumbnail()
    {
        if (prefab != null)
        {
            Texture2D thumbnailTexture = UnityEditor.AssetPreview.GetAssetPreview(prefab);
            if (thumbnailTexture != null)
            {
                // Get the background color from the first pixel
                Color backgroundColor = thumbnailTexture.GetPixel(0, 0);

                // Create a new Texture2D with transparent background
                Texture2D processedTexture = new Texture2D(thumbnailTexture.width, thumbnailTexture.height, TextureFormat.RGBA32, false);

                // Get all pixels from the original thumbnail texture
                Color[] pixels = thumbnailTexture.GetPixels();

                // Set transparent pixels where the original texture was the background color
                for (int i = 0; i < pixels.Length; i++)
                {
                    if (pixels[i] == backgroundColor)
                    {
                        pixels[i] = Color.clear;
                    }
                }

                // Set the pixels to the processed texture and apply changes
                processedTexture.SetPixels(pixels);
                processedTexture.Apply();

                // Create the Sprite with the processed texture
                Sprite thumbnailSprite = Sprite.Create(processedTexture, new Rect(0, 0, processedTexture.width, processedTexture.height), Vector2.one * 0.5f);

                // Update the UI Image with the processed thumbnail
                icon.sprite = thumbnailSprite;
            }
        }
    }

    private string AddSpacesToPrefabName(string prefabName)
    {
        string spacedName = string.Empty;

        for (int i = 0; i < prefabName.Length; i++)
        {
            if (i > 0 && char.IsUpper(prefabName[i]))
            {
                spacedName += " ";
            }

            spacedName += prefabName[i];
        }

        return spacedName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CypherCode
{
    public class ObjectUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public GameObject prefab;
        public Image UIThumbnail;
        public CaptureLevel levelRoot;

        private Transform draggingInstance;
        private GameObject highlight;

        private void Start()
        {
            highlight = FindObjectOfType<ObjectHighlightHandler>().gameObject;
            levelRoot = FindObjectOfType<CaptureLevel>().GetComponent<CaptureLevel>();
        }

        public void UpdateThumbnail()
        {
            if (prefab != null)
            {
                ObjectsSO objectsSO = Resources.Load<ObjectsSO>("ObjectsList");
                Texture2D thumbnailTexture = objectsSO.FindThumbnail(prefab);
                if (thumbnailTexture != null)
                {
                    // Get the background color from the first pixel
                    Color backgroundColor = new Color(82f / 255f, 82f / 255f, 82f / 255f, 1f);

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
                    UIThumbnail.sprite = thumbnailSprite;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(ThumbnailGridGen.thereIsAFinishLine && prefab.name.Contains("Finish"))
            {
                return;
            }
            else
            {
                ThumbnailGridGen.thereIsAFinishLine = true;
            }

            // When the user starts dragging, create the prefab instance
            if (prefab != null)
            {
                draggingInstance = Instantiate(prefab, levelRoot.levelRoot.transform.GetChild(0).transform).transform;

                // Start dragging the newly created instance
                ObjectMover mover = draggingInstance.gameObject.AddComponent<ObjectMover>();
                if (mover != null)
                {
                    FindObjectOfType<LEDescriptor>().GetComponent<LEDescriptor>().SetObjDesc(prefab);
                    mover.Select();
                    highlight.GetComponent<ObjectHighlightHandler>().SetParent(draggingInstance.gameObject);
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // When the user releases the drag, place the prefab instance in the game world
            if (draggingInstance != null)
            {

                // Enable colliders and scripts on the placed instance
                //SetCoreComponentStatus(true);

                // Stop dragging the placed instance
                ObjectMover mover = draggingInstance.gameObject.GetComponent<ObjectMover>();
                if (mover != null)
                {
                    mover.Deselect();
                }

                draggingInstance = null;
            }
        }
    }
}

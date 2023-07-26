using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CypherCode
{
    public class ObjectSelector : MonoBehaviour
    {
        private Camera mainCamera;
        private ObjectMover selectedObject;
        private LEDescriptor descriptor;
        private GameObject highlight;

        private void Start()
        {
            mainCamera = Camera.main;
            descriptor = FindObjectOfType<LEDescriptor>().GetComponent<LEDescriptor>();
            highlight = FindObjectOfType<ObjectHighlightHandler>().gameObject;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    ObjectMover objectMover = hit.collider.GetComponent<ObjectMover>();
                    if (objectMover != null)
                    {
                        if (selectedObject != null)
                        {
                            // Deselect the previously selected object
                            selectedObject.Deselect();
                        }

                        // Select the new object
                        selectedObject = objectMover;
                        selectedObject.Select();
                        descriptor.SetObjDesc(selectedObject.gameObject);
                        highlight.GetComponent<ObjectHighlightHandler>().SetParent(selectedObject.gameObject);
                    }
                }
                else if (selectedObject != null)
                {
                    // If clicked outside an object, deselect the currently selected object
                    selectedObject.Deselect();
                    selectedObject = null;
                }
            }

            // Reset the position and rotation of the selected object when R key is pressed
            if (Input.GetKeyDown(KeyCode.R) && selectedObject != null)
            {
                selectedObject.ResetPositionAndRotation();
            }
        }
    }
}

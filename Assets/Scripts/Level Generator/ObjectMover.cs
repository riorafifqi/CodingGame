using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CypherCode
{
    public class ObjectMover : MonoBehaviour
    {
        private Camera mainCamera;
        
        //flags
        private bool isDragging = false;
        private bool isSelected = false;
        private Vector2 lastPointerScreenPos;
        
        //for undos
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        
        
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private const float positionInterpolationSpeed = 10f;

        //core component
        public Collider[] colliders;
        public MonoBehaviour[] scripts;

        private void Start()
        {
            mainCamera = Camera.main;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            targetPosition = initialPosition;
            colliders = GetComponentsInChildren<Collider>();
            scripts = GetComponentsInChildren<MonoBehaviour>();
        }

        public void Select()
        {
            isSelected = true;
            isDragging = true;
        }

        public void Deselect()
        {
            isSelected = false;
            isDragging = false;
            SetCoreComponentStatus(true);
        }

        public void StopDragging()
        {
            if (isDragging) isDragging = false;
        }

        private void Update()
        {
            if (isSelected)
            {
                HandleMovementInput();
                HandleRotationInput();
                HandleHeightAdjustmentInput();
            }
            if (isDragging)
            {
                SetCoreComponentStatus(false);
                // Interpolate towards the target position for smoother movement
            }
            else
            {
                SetCoreComponentStatus(true);
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionInterpolationSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * positionInterpolationSpeed);

        }
        
        private Vector3 GetPointerWorldPosition(Vector2 screenPos)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }

            // Default to the plane at y = 0 if no collision occurred
            float t = -ray.origin.y / ray.direction.y;
            return ray.origin + t * ray.direction;
        }

        private bool CheckCollision(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapBox(position, transform.localScale * 0.4f);

            foreach (var collider in colliders)
            {
                if (collider != GetComponent<Collider>())
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleMovementInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastPointerScreenPos = Input.mousePosition;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if(isDragging)
            { 
                Vector2 pointerScreenPos = Input.mousePosition;
                bool isVirus = transform.name.Contains("Virus");

                // Check if the pointer has moved since the last frame
                bool pointerMoved = pointerScreenPos != lastPointerScreenPos;
                lastPointerScreenPos = pointerScreenPos;

                // Convert the pointer position to world space
                Vector3 pointerWorldPos = GetPointerWorldPosition(pointerScreenPos);

                // Adjust the height of the object based on the isometric position
                pointerWorldPos.y = transform.localScale.y * 0.5f; // Set y to half of the object's height

                // Perform continuous collision check and stacking
                while (CheckCollision(pointerWorldPos))
                {
                    pointerWorldPos.y += 1; // Stack on top of the object below
                }

                if (isVirus)
                {
                    pointerWorldPos.y -= 1;
                }

                // Snap the position to the grid (optional - if you have a grid-based level editor)
                pointerWorldPos.x = Mathf.Round(pointerWorldPos.x);
                pointerWorldPos.z = Mathf.Round(pointerWorldPos.z);

                // Clamp X and Z axis to the bounds of the level
                pointerWorldPos.x = Mathf.Clamp(pointerWorldPos.x, -5f, 5f);
                pointerWorldPos.z = Mathf.Clamp(pointerWorldPos.z, -5f, 5f);

                if (pointerMoved)
                    targetPosition = pointerWorldPos;

            }
        }

        private void HandleRotationInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateObject(-90f);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateObject(90f);
            }
        }

        private void RotateObject(float angle)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + angle, currentRotation.z);
            targetRotation = Quaternion.Euler(newRotation);
        }

        private void HandleHeightAdjustmentInput()
        {
            float heightChangeIncrement = 0.5f; // Height change increment

            if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3 newPosition = transform.position + Vector3.up * heightChangeIncrement;
                newPosition.y += 0.5f;

                // Snap to nearest height change increment
                newPosition.y = SnapToIncrement(newPosition.y, heightChangeIncrement);

                // Clamp the height to the minimum allowed
                float minHeight = -0.5f;
                newPosition.y = Mathf.Max(newPosition.y, minHeight);

                if (!CheckCollision(newPosition))
                    targetPosition = newPosition;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3 newPosition = transform.position + (Vector3.down * heightChangeIncrement);
                newPosition.y -= 0.5f;

                // Snap to nearest height change increment
                newPosition.y = SnapToIncrement(newPosition.y, heightChangeIncrement);

                // Clamp the height to the minimum allowed
                float minHeight = -0.5f;
                newPosition.y = Mathf.Max(newPosition.y, minHeight);
                if (!CheckCollision(newPosition))
                    targetPosition = newPosition;
            }
        }

        private float SnapToIncrement(float value, float increment)
        {
            return Mathf.Round(value / increment) * increment;
        }

        public void ResetPositionAndRotation()
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        private void SetCoreComponentStatus(bool status)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = status;
            }

            foreach (var script in scripts)
            {
                if(script == this)
                    continue;
                script.enabled = status;
            }
        }
    }
}

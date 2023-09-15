using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CypherCode
{
    public class RotationColorChange : MonoBehaviour
    {
        public Color color;
        public Material material;

        // Start is called before the first frame update
        void Awake()
        {
            material = GetComponent<Renderer>().material;
            color = material.color;
        }

        // Update is called once per frame
        void Update()
        {
            // Get the current rotation of the object on the y-axis
            float rotationY = transform.transform.rotation.eulerAngles.y;

            // Normalize the rotation value to a range between 0 and 1
            float normalizedRotation = Mathf.InverseLerp(0f, 360f, rotationY);

            // Set the material color based on the normalized rotation
            //material.color = Color.HSVToRGB(normalizedRotation, 1f, 1f);
            color.b = normalizedRotation;
            material.color = color;
            //Debug.Log(material.color);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CypherCode
{
    public class ObjectHighlightHandler : MonoBehaviour
    {
        public void SetParent(GameObject parent)
        {
            transform.position = parent.transform.position;
            transform.SetParent(parent.transform);
        }
    }
}

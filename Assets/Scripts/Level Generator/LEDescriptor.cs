using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace CypherCode
{
    public class LEDescriptor : MonoBehaviour
    {
        public GameObject objDesc;
        public TMP_Text objName;
        public TMP_Text objDescText;

        GameObjectDescription objDescScript;

        private void Start()
        {
            objName.text = "";
            objDescText.text = "";
            objDesc = null;
        }

        public void SetObjDesc(GameObject obj)
        {
            objDesc = obj;
            UpdateObject();
        }

        void UpdateObject()
        {
            if (objDesc.GetComponent<GameObjectDescription>() != null)
            {
                objDescScript = objDesc.GetComponent<GameObjectDescription>();

                string[] data = ParseSentences(objDescScript.description);
                objName.text = data[0];
                objDescText.text = data[1];
            }    
            else
            {
                //Debug.Log("No OBj" + objDesc.name);
                if (objDesc.name.Contains("Floor"))
                {
                    //Debug.Log("this is floor");
                    objName.text = "Floor";
                    objDescText.text = "A floor tile, solid, you can stand on this.";
                }
            }
        }

        string[] ParseSentences(string input)
        {
            // Split the input string using the colon (':') as the separator
            string[] sentences = input.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            return sentences;
        }
    }
}

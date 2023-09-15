using System.IO;
using UnityEngine;

namespace CypherCode
{
    public class CustomLevelLoader : MonoBehaviour
    {
        public GameObject levelUIPrefab;

        string folderName = "CypherCodeCustoms";
        string path;

        public static string selectedLevelFileName { get; set; }

        void OnEnable()
        {
            GenerateCustomLevelList();
        }

        void GenerateCustomLevelList()
        {
            path = Path.Combine(Application.persistentDataPath, folderName);

            foreach (var oldLevel in GetComponentsInChildren<CustomLevelItem>())
            {
                Destroy(oldLevel.gameObject);
            }

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    if (IsImageFile(file))
                    {
                        string fileName = Path.GetFileName(file);
                        string creationDate = File.GetCreationTime(file).ToString("dd_MM_yyyy");
                        //Debug.Log("Image File Name: " + fileName + "_" + creationDate);

                        CustomLevelItem temp = Instantiate(levelUIPrefab, transform).GetComponent<CustomLevelItem>();
                        temp.fullLevelName = fileName + "_" +creationDate;
                    }
                }
            }
            else
            {
                Debug.LogWarning("The folder " + folderName + " does not exist in the persistent data path.");
            }
        }

        private bool IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg"};
            string fileExtension = Path.GetExtension(filePath).ToLower();

            foreach (string extension in imageExtensions)
            {
                if (fileExtension == extension)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateSelected()
        {
            foreach (var temp in GetComponentsInChildren<CustomLevelItem>())
            {
                if (temp.fullLevelName != selectedLevelFileName)
                {
                    temp.selected.SetActive(false);
                    //temp.levelUIName.color = Color.white;
                }
                else
                {
                    temp.selected.SetActive(true);
                    //temp.levelUIName.color = Color.black;
                }
            }
        }
    }
}

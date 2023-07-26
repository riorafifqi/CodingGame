using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CypherCode
{
    public class CustomLevelInspector : MonoBehaviour
    {
        public TMP_Text levelCreator;
        public TMP_Text levelDate;

        void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            if(CustomLevelLoader.selectedLevelFileName != null)
            {
                string[] parts = CustomLevelLoader.selectedLevelFileName.Split('_');
                levelCreator.text = parts[3].Split('.')[0];
                levelDate.text = parts[4] + "/" + parts[5] + "/" + parts[6];
            }
        }

        public void PlayLevel()
        {
            if(CustomLevelLoader.selectedLevelFileName != "")
                SceneManager.LoadScene("CustomLevel");
        }

        public void CreateCustomLevel()
        {
            SceneManager.LoadScene("LevelEditor");
        }
    }
}

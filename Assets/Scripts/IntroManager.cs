using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CypherCode
{
    public class IntroManager : MonoBehaviour
    {
        private VideoPlayer cutscene;
        private Text clickText;

        // Start is called before the first frame update
        void Start()
        {
            cutscene = GetComponentInChildren<VideoPlayer>();
            clickText = GetComponentInChildren<Text>();

            clickText.enabled = false;
            StartCoroutine(StartBlinking());
            cutscene.loopPointReached += DisableIntro;

            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DisableIntro(cutscene);
            }
        }

        private void ToggleVisibility()
        {
            clickText.enabled = !clickText.enabled;
        }

        private IEnumerator StartBlinking()
        {
            yield return new WaitForSeconds(5f);

            InvokeRepeating("ToggleVisibility", 0, 1f);
        }

        private void DisableIntro(UnityEngine.Video.VideoPlayer vp)
        {
            vp.Stop();
            this.gameObject.SetActive(false);
        }
    }
}

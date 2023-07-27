using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CypherCode
{
    public class SavingScreen : MonoBehaviour
    {
        public TMP_Text savingText;
        public TMP_Text loadingText;
        public float animationSpeed = 0.5f; // Adjust the speed of the animation.
        public string[] loadingTips;
        public float tipChangeInterval = 2.0f;

        void OnEnable()
        {
            StartCoroutine(AnimateEllipsis());
            StartCoroutine(ChangeLoadingTip());
        }

        IEnumerator AnimateEllipsis()
        {
            string tempText = savingText.text;
            while (true)
            {
                savingText.text = tempText;
                // Animation: Add three dots with a delay.
                yield return new WaitForSeconds(animationSpeed);
                savingText.text = tempText + ".";
                yield return new WaitForSeconds(animationSpeed);
                savingText.text = tempText + "..";
                yield return new WaitForSeconds(animationSpeed);
                savingText.text = tempText + "...";
                yield return new WaitForSeconds(animationSpeed);

                // Repeat the animation.
            }
        }

        IEnumerator ChangeLoadingTip()
        {
            int currentIndex = 0;

            while (true)
            {
                if (currentIndex >= loadingTips.Length)
                {
                    currentIndex = 1;
                }

                int randomIndex = Random.Range(1, loadingTips.Length);
                if(currentIndex != 0)
                    loadingText.text = loadingTips[randomIndex];
                else
                    loadingText.text = loadingTips[0];

                currentIndex++;

                yield return new WaitForSeconds(tipChangeInterval);
            }
        }
    }
}

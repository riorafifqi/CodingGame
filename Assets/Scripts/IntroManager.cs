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
        private bool hasClicked = false;

        [SerializeField] RawImage videoRawImage;

        // Menu Element
        [SerializeField] RectTransform topMenu;
        [SerializeField] RectTransform bottomMenu;
        [SerializeField] RectTransform profileMenu;
        Vector2 initPosTop;
        Vector2 initPosBot;
        Vector2 initPosProfile;

        // Start is called before the first frame update
        void Start()
        {
            MenuOnStart();

            cutscene = GetComponentInChildren<VideoPlayer>();
            clickText = GetComponentInChildren<Text>();

            clickText.enabled = false;
            StartCoroutine(StartBlinking());
            cutscene.loopPointReached += DisableIntro;

            if (hasClicked)
                gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShowMenu();
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
            StartCoroutine(CloseCutscene());
            hasClicked = true;
        }

        private void MenuOnStart()
        {
            initPosTop = topMenu.anchoredPosition;
            initPosBot = bottomMenu.anchoredPosition;
            initPosProfile = profileMenu.anchoredPosition;

            topMenu.anchoredPosition = new Vector2(topMenu.anchoredPosition.x, Screen.height);
            bottomMenu.anchoredPosition = new Vector2(bottomMenu.anchoredPosition.x, -Screen.height);
            profileMenu.anchoredPosition = new Vector2(-Screen.width, profileMenu.anchoredPosition.y);
        }

        private void ShowMenu()
        {
            StartCoroutine(MoveUIElementCoroutine(topMenu, initPosTop));
            StartCoroutine(MoveUIElementCoroutine(bottomMenu, initPosBot));
            StartCoroutine(MoveUIElementCoroutine(profileMenu, initPosProfile));
        }

        private IEnumerator MoveUIElementCoroutine(RectTransform targetObject, Vector2 targetPosition)
        {
            float moveSpeed = 500f;

            // Calculate the distance between the current position and the target position
            float distance = Vector2.Distance(targetObject.anchoredPosition, targetPosition);

            // Calculate the time it should take to reach the target position based on moveSpeed
            float duration = distance / moveSpeed;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // Move the UI element towards the target position using Lerp
                targetObject.anchoredPosition = Vector2.Lerp(targetObject.anchoredPosition, targetPosition, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            // Ensure the UI element is exactly at the target position
            targetObject.anchoredPosition = targetPosition;
        }

        private IEnumerator CloseCutscene()
        {
            float fadeDuration = 2f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                Color targetColor = videoRawImage.color;
                targetColor.a = 0;
                // Calculate the new color with interpolated alpha value
                Color newColor = Color.Lerp(videoRawImage.color, targetColor, elapsedTime / fadeDuration);

                // Apply the new color to the UI element
                videoRawImage.color = newColor;

                // Increment the elapsed time
                elapsedTime += Time.deltaTime;

                yield return null; // Wait for the next frame
            }

            // Perform actions when the fade is complete (e.g., disable the object)
            gameObject.SetActive(false);
        }
    }
}

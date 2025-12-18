using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class ScreenFadeManager : MonoBehaviour
    {
        public static ScreenFadeManager Instance;

        public Image fadeImage;
        public float fadeDuration = 0.5f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            fadeImage.gameObject.SetActive(false);
        }

        public IEnumerator FadeOut()
        {
            fadeImage.gameObject.SetActive(true);
            yield return Fade(0f, 1f);
        }

        public IEnumerator FadeIn()
        {
            yield return Fade(1f, 0f);
            fadeImage.gameObject.SetActive(false);
        }

        private IEnumerator Fade(float from, float to)
        {
            float t = 0f;
            Color c = fadeImage.color;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(from, to, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }

            c.a = to;
            fadeImage.color = c;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class StartTextManager : MonoBehaviour
    {
        public Text startText;
        public float textDuration = 2f;
        public float fadeDuration = 1f;

        private void Start()
        {
            StartCoroutine(ShowStartText());
        }

        IEnumerator ShowStartText()
        {
            // 텍스트 기본값 설정
            startText.gameObject.SetActive(true);
            Color originalColor = startText.color;
            originalColor.a = 1f;
            startText.color = originalColor;

            yield return new WaitForSeconds(textDuration);

            // 페이드 아웃
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                startText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            startText.gameObject.SetActive(false);
        }
    }
}

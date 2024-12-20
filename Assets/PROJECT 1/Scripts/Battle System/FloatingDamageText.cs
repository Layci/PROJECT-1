using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class FloatingDamageText : MonoBehaviour
    {
        private Text damageText; // Text 컴포넌트를 참조
        public float moveSpeed = 2f; // 텍스트 이동 속도
        public float fadeDuration = 1f; // 텍스트 사라지는 시간

        private Color textColor; // 텍스트 색상 저장

        private void Awake()
        {
            // Text 컴포넌트를 동적으로 가져옴
            damageText = GetComponent<Text>();
            if (damageText == null)
            {
                Debug.LogError("Text 컴포넌트를 찾을 수 없습니다!");
                return;
            }
        }

        private void Start()
        {
            // 초기 색상 저장
            if (damageText != null)
            {
                textColor = damageText.color;
            }
        }

        private void Update()
        {
            if (damageText == null) return;

            // 위로 이동
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            // 페이드 아웃
            textColor.a -= Time.deltaTime / fadeDuration;
            damageText.color = textColor;

            // 페이드가 끝나면 오브젝트 제거
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        // 텍스트를 초기화하는 메서드
        public void SetDamage(int damage, Color color)
        {
            if (damageText == null) return;

            damageText.text = damage.ToString(); // 대미지 값 설정
            textColor = color; // 텍스트 색상 설정
            damageText.color = textColor; // 텍스트에 색상 적용
        }
    }
}

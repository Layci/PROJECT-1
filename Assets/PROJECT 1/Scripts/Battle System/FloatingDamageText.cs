 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class FloatingDamageText : MonoBehaviour
    {
        public GameObject canvasPrefab;  // 캔버스 프리팹
        private Text damageText;
        public float moveSpeed = 2f; // 텍스트 이동 속도
        public float fadeDuration = 1f; // 텍스트 사라지는 시간


        private Color textColor; // 텍스트 색상 저장

        private void Start()
        {
            // 캔버스 프리팹에서 텍스트 오브젝트 찾기
            damageText = canvasPrefab.GetComponentInChildren<Text>();

            if (damageText == null)
            {
                Debug.LogError("캔버스 프리팹에서 Text 컴포넌트를 찾을 수 없습니다!");
                return;
            }

            textColor = damageText.color; // 초기 색상 저장
        }

        private void Update()
        {
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
        public void SetDamage(int damage)
        {
            damageText.text = damage.ToString(); // 대미지 값 설정
        }
    }
}

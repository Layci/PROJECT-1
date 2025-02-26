using Project1;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
    public class DamageTextSpawner : MonoBehaviour
    {
        public static DamageTextSpawner Instance; // 싱글턴 인스턴스
        public GameObject damageTextPrefab; // 텍스트만 있는 프리팹
        public Canvas worldCanvas; // 월드 캔버스 참조

        private void Awake()
        {
            // 싱글턴 초기화
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject); // 중복된 인스턴스 방지
            }
        }

        public void SpawnDamageText(Vector3 position, int damage)
        {
            // 텍스트 프리팹 생성
            GameObject damageTextObj = Instantiate(damageTextPrefab, worldCanvas.transform);

            // Text 컴포넌트에 데미지 값 설정
            FloatingDamageText floatingDamageText = damageTextObj.GetComponent<FloatingDamageText>();
            if (floatingDamageText != null)
            {
                floatingDamageText.SetDamage(damage, Color.yellow); // 대미지 값과 색상 설정
            }

            // RectTransform 위치 설정
            RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 월드 좌표를 캔버스 로컬 좌표로 변환
                Vector2 canvasPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    worldCanvas.GetComponent<RectTransform>(),
                    Camera.main.WorldToScreenPoint(position), // 월드 좌표 -> 스크린 좌표
                    Camera.main,
                    out canvasPosition // 스크린 좌표 -> 캔버스 좌표
                );

                rectTransform.localPosition = canvasPosition + new Vector2(0, 7); // 약간 위로 올림
            }

            // 일정 시간 후 삭제
            Destroy(damageTextObj, 2f);
        }
    }
}

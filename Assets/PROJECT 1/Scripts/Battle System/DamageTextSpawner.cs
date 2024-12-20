using UnityEngine;
using UnityEngine.UI;

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
        Text textComponent = damageTextObj.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = damage.ToString();
        }
        else
        {
            Debug.LogError("Text 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        // RectTransform 위치 설정
        RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 월드 좌표를 로컬 캔버스 좌표로 변환
            rectTransform.position = position + new Vector3(0, 2, 0); // 약간 위로 올림
        }
        else
        {
            Debug.LogError("RectTransform을 찾을 수 없습니다!");
        }

        // 일정 시간 후 삭제
        Destroy(damageTextObj, 2f);
    }
}

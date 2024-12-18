using UnityEngine;
using UnityEngine.UI;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance; // 싱글턴 인스턴스
    public GameObject damageTextPrefab;       // DamageText 프리팹
    public Canvas worldCanvas;                // 월드 스페이스 캔버스

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
        // DamageText 프리팹 생성
        GameObject instance = Instantiate(damageTextPrefab, worldCanvas.transform);

        // 텍스트 위치 설정
        instance.transform.position = position;

        // 텍스트 초기화
        Text damageText = instance.GetComponent<Text>();
        if (damageText != null)
        {
            damageText.text = damage.ToString();
        }

        // 일정 시간 후 삭제
        //Destroy(instance, 1.5f); // 1.5초 후 삭제
    }
}

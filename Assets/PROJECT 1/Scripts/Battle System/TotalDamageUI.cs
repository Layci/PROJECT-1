using Project1;
using UnityEngine;
using UnityEngine.UI;

public class TotalDamageUI : MonoBehaviour
{
    public static TotalDamageUI Instance; // 싱글턴

    Text totalDamageText; // 총 데미지를 표시할 텍스트
    public Text subDamageText; // 총 피해량을 표시할 텍스트
    public float displayDuration = 2f; // UI가 표시되는 시간
    public float currentTotalDamage = 0f;

    private float timer = 0f;
    public bool isVisible = false;

    private void Awake()
    {
        // Text 컴포넌트 가져오기
        totalDamageText = GetComponent<Text>();

        // 싱글턴 패턴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // UI가 보이는 동안 타이머를 감소
        if (isVisible)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                HideDamageUI(); // 시간이 다 되면 UI 숨김
            }
        }
    }

    // 총 데미지를 설정하고 UI를 표시하는 메서드
    public void ShowTotalDamage(int totalDamage)
    {
        //totalDamageText.text = $"Total Damage: {totalDamage}"; // 텍스트 업데이트
        currentTotalDamage = totalDamage;
        totalDamageText.text = currentTotalDamage.ToString();
        totalDamageText.gameObject.SetActive(true); // 텍스트 표시
        subDamageText.gameObject.SetActive(true); // 텍스트 표시

        timer = displayDuration; // 표시 시간을 초기화
        isVisible = true;
        totalDamage = 0;
    }

    // UI를 숨기는 메서드
    private void HideDamageUI()
    {
        totalDamageText.gameObject.SetActive(false); // 텍스트 숨김
        subDamageText.gameObject.SetActive(false); // 텍스트 숨김
        isVisible = false;

        // 내부 값 및 UI 텍스트 초기화
        currentTotalDamage = 0;
    }
}

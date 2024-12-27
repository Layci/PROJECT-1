using Project1;
using UnityEngine;
using UnityEngine.UI;

public class TotalDamageUI : MonoBehaviour
{
    public static TotalDamageUI Instance; // �̱���

    Text totalDamageText; // �� �������� ǥ���� �ؽ�Ʈ
    public Text subDamageText; // �� ���ط��� ǥ���� �ؽ�Ʈ
    public float displayDuration = 2f; // UI�� ǥ�õǴ� �ð�
    public float currentTotalDamage = 0f;

    private float timer = 0f;
    public bool isVisible = false;

    private void Awake()
    {
        // Text ������Ʈ ��������
        totalDamageText = GetComponent<Text>();

        // �̱��� ����
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
        // UI�� ���̴� ���� Ÿ�̸Ӹ� ����
        if (isVisible)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                HideDamageUI(); // �ð��� �� �Ǹ� UI ����
            }
        }
    }

    // �� �������� �����ϰ� UI�� ǥ���ϴ� �޼���
    public void ShowTotalDamage(int totalDamage)
    {
        //totalDamageText.text = $"Total Damage: {totalDamage}"; // �ؽ�Ʈ ������Ʈ
        currentTotalDamage = totalDamage;
        totalDamageText.text = currentTotalDamage.ToString();
        totalDamageText.gameObject.SetActive(true); // �ؽ�Ʈ ǥ��
        subDamageText.gameObject.SetActive(true); // �ؽ�Ʈ ǥ��

        timer = displayDuration; // ǥ�� �ð��� �ʱ�ȭ
        isVisible = true;
        totalDamage = 0;
    }

    // UI�� ����� �޼���
    private void HideDamageUI()
    {
        totalDamageText.gameObject.SetActive(false); // �ؽ�Ʈ ����
        subDamageText.gameObject.SetActive(false); // �ؽ�Ʈ ����
        isVisible = false;

        // ���� �� �� UI �ؽ�Ʈ �ʱ�ȭ
        currentTotalDamage = 0;
    }
}

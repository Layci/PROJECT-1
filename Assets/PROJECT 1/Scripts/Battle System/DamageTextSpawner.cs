using UnityEngine;
using UnityEngine.UI;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance; // �̱��� �ν��Ͻ�
    public GameObject damageTextPrefab;       // DamageText ������
    public Canvas worldCanvas;                // ���� �����̽� ĵ����

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
        }
    }

    public void SpawnDamageText(Vector3 position, int damage)
    {
        // DamageText ������ ����
        GameObject instance = Instantiate(damageTextPrefab, worldCanvas.transform);

        // �ؽ�Ʈ ��ġ ����
        instance.transform.position = position;

        // �ؽ�Ʈ �ʱ�ȭ
        Text damageText = instance.GetComponent<Text>();
        if (damageText != null)
        {
            damageText.text = damage.ToString();
        }

        // ���� �ð� �� ����
        //Destroy(instance, 1.5f); // 1.5�� �� ����
    }
}

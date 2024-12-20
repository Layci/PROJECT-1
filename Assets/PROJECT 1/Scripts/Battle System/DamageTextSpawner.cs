using UnityEngine;
using UnityEngine.UI;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance; // �̱��� �ν��Ͻ�
    public GameObject damageTextPrefab; // �ؽ�Ʈ�� �ִ� ������
    public Canvas worldCanvas; // ���� ĵ���� ����

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
        // �ؽ�Ʈ ������ ����
        GameObject damageTextObj = Instantiate(damageTextPrefab, worldCanvas.transform);

        // Text ������Ʈ�� ������ �� ����
        Text textComponent = damageTextObj.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = damage.ToString();
        }
        else
        {
            Debug.LogError("Text ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        // RectTransform ��ġ ����
        RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // ���� ��ǥ�� ���� ĵ���� ��ǥ�� ��ȯ
            rectTransform.position = position + new Vector3(0, 2, 0); // �ణ ���� �ø�
        }
        else
        {
            Debug.LogError("RectTransform�� ã�� �� �����ϴ�!");
        }

        // ���� �ð� �� ����
        Destroy(damageTextObj, 2f);
    }
}

using Project1;
using UnityEngine;
using UnityEngine.UI;

namespace Project1
{
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
            FloatingDamageText floatingDamageText = damageTextObj.GetComponent<FloatingDamageText>();
            if (floatingDamageText != null)
            {
                floatingDamageText.SetDamage(damage, Color.yellow); // ����� ���� ���� ����
            }

            // RectTransform ��ġ ����
            RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // ���� ��ǥ�� ĵ���� ���� ��ǥ�� ��ȯ
                Vector2 canvasPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    worldCanvas.GetComponent<RectTransform>(),
                    Camera.main.WorldToScreenPoint(position), // ���� ��ǥ -> ��ũ�� ��ǥ
                    Camera.main,
                    out canvasPosition // ��ũ�� ��ǥ -> ĵ���� ��ǥ
                );

                rectTransform.localPosition = canvasPosition + new Vector2(0, 7); // �ణ ���� �ø�
            }

            // ���� �ð� �� ����
            Destroy(damageTextObj, 2f);
        }
    }
}

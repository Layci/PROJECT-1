using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class FloatingDamageText : MonoBehaviour
    {
        private Text damageText; // Text ������Ʈ�� ����
        public float moveSpeed = 2f; // �ؽ�Ʈ �̵� �ӵ�
        public float fadeDuration = 1f; // �ؽ�Ʈ ������� �ð�

        private Color textColor; // �ؽ�Ʈ ���� ����

        private void Awake()
        {
            // Text ������Ʈ�� �������� ������
            damageText = GetComponent<Text>();
            if (damageText == null)
            {
                Debug.LogError("Text ������Ʈ�� ã�� �� �����ϴ�!");
                return;
            }
        }

        private void Start()
        {
            // �ʱ� ���� ����
            if (damageText != null)
            {
                textColor = damageText.color;
            }
        }

        private void Update()
        {
            if (damageText == null) return;

            // ���� �̵�
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            // ���̵� �ƿ�
            textColor.a -= Time.deltaTime / fadeDuration;
            damageText.color = textColor;

            // ���̵尡 ������ ������Ʈ ����
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        // �ؽ�Ʈ�� �ʱ�ȭ�ϴ� �޼���
        public void SetDamage(int damage, Color color)
        {
            if (damageText == null) return;

            damageText.text = damage.ToString(); // ����� �� ����
            textColor = color; // �ؽ�Ʈ ���� ����
            damageText.color = textColor; // �ؽ�Ʈ�� ���� ����
        }
    }
}

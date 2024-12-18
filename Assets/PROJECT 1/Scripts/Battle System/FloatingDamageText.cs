 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class FloatingDamageText : MonoBehaviour
    {
        public GameObject canvasPrefab;  // ĵ���� ������
        private Text damageText;
        public float moveSpeed = 2f; // �ؽ�Ʈ �̵� �ӵ�
        public float fadeDuration = 1f; // �ؽ�Ʈ ������� �ð�


        private Color textColor; // �ؽ�Ʈ ���� ����

        private void Start()
        {
            // ĵ���� �����տ��� �ؽ�Ʈ ������Ʈ ã��
            damageText = canvasPrefab.GetComponentInChildren<Text>();

            if (damageText == null)
            {
                Debug.LogError("ĵ���� �����տ��� Text ������Ʈ�� ã�� �� �����ϴ�!");
                return;
            }

            textColor = damageText.color; // �ʱ� ���� ����
        }

        private void Update()
        {
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
        public void SetDamage(int damage)
        {
            damageText.text = damage.ToString(); // ����� �� ����
        }
    }
}

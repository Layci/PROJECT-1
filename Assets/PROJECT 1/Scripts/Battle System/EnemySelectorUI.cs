using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project1
{
    public class EnemySelectorUI : MonoBehaviour
    {
        public RectTransform selectorUI; // ���� UI ������Ʈ
        public Camera mainCamera;        // ���� ī�޶� ����
        public bool isTurn = false;
        public static EnemySelectorUI instance;
        public float yOffset = 50f;    // Y���� �ø� ������ ��

        public Transform selectedEnemy; // ���� ���õ� ���� Transform

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main; // ���� ī�޶� ����
            }
        }

        private void Update()
        {
            if (selectedEnemy != null && isTurn)
            {
                // ���õ� ���� ��ġ�� UI�� ��ȯ
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(selectedEnemy.transform.position);
                screenPosition.y += yOffset;

                // UI�� ��ġ ����
                selectorUI.position = screenPosition;
            }
            else if (!isTurn)
            {
                // ���õ� ���� ������ UI�� ����
                selectorUI.gameObject.SetActive(false);
            }

            if(isTurn)
            {
                // �÷��̾� ���̸� UI�� ���̰� ��
                selectorUI.gameObject.SetActive(true);
            }
        }

        // ���õ� �� ����
        public void SetSelectedEnemy(Transform enemyTransform)
        {
            selectedEnemy = enemyTransform;

            if (selectedEnemy != null)
            {
                selectorUI.gameObject.SetActive(true); // UI Ȱ��ȭ
            }
        }

        // ���� ���
        public void DeselectEnemy()
        {
            selectedEnemy = null;
            selectorUI.gameObject.SetActive(false);
        }
    }
}

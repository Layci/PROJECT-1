using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemySelectorUI : MonoBehaviour
    {
        public RectTransform selectorUI; // ���� UI ������Ʈ
        public Camera mainCamera;        // ���� ī�޶� ����
        public bool isTurn = false;
        public static EnemySelectorUI instance;

        private Transform selectedEnemy; // ���� ���õ� ���� Transform

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
            if (selectedEnemy != null)
            {
                // ���õ� ���� ��ġ�� UI�� ��ȯ
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(selectedEnemy.position);

                // UI�� ��ġ ����
                selectorUI.position = screenPosition;
            }
            else
            {
                // ���õ� ���� ������ UI�� ����
                selectorUI.gameObject.SetActive(false);
            }

            if (!isTurn)
            {
                // �÷��̾� ���� �ƴϸ� UI�� ����
                selectorUI.gameObject.SetActive(false);
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

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
        public List<RectTransform> multiSelectorUIs = new List<RectTransform>();

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

        public void ShowSingleTargetUI()
        {
            selectorUI.gameObject.SetActive(true);
        }

        public void HideSingleTargetUI()
        {
            selectorUI.gameObject.SetActive(false);
        }

        public void ShowAOETargets(List<Transform> aoeTargets)
        {
            // ���� UI ����
            selectorUI.gameObject.SetActive(false);

            // �ʿ� ����ŭ UI ������Ʈ ���� �Ǵ� ����
            while (multiSelectorUIs.Count < aoeTargets.Count)
            {
                RectTransform newUI = Instantiate(selectorUI, selectorUI.transform.parent);
                multiSelectorUIs.Add(newUI);
            }

            for (int i = 0; i < multiSelectorUIs.Count; i++)
            {
                if (i < aoeTargets.Count)
                {
                    Transform enemy = aoeTargets[i];
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position);
                    screenPos.y += yOffset;

                    multiSelectorUIs[i].position = screenPos;
                    multiSelectorUIs[i].gameObject.SetActive(true);
                }
                else
                {
                    multiSelectorUIs[i].gameObject.SetActive(false);
                }
            }
        }

        public void HideAOEUI()
        {
            foreach (var ui in multiSelectorUIs)
            {
                ui.gameObject.SetActive(false);
            }
        }

        public List<Transform> selectedEnemies = new List<Transform>();

        public void HighlightEnemies(List<Transform> targets)
        {
            selectedEnemies = targets;

            // ����: �� �� ���� ���̶���Ʈ ǥ�� ������Ʈ Ȱ��ȭ
            foreach (var enemy in selectedEnemies)
            {
                // ���̶���Ʈ ǥ�� ������Ʈ�� �� ���� ���� ���� �߰�
                // ��: enemy.GetComponent<EnemyUI>()?.ShowHighlight();
            }

            selectorUI.gameObject.SetActive(false); // ���� ���� UI�� ����
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

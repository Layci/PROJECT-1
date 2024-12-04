using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemySelection : MonoBehaviour
    {
        public TurnSystem turnSystem;  // �� �ý��� ����
        public EnemySelectorUI enemySelectorUI; // ���� UI ���� ��ũ��Ʈ
        private int selectedEnemyIndex = 0;     // ���� ���õ� ���� �ε���

        private void Start()
        {
            EnemySelectorUI.instance.selectorUI.gameObject.SetActive(true);

            // ó�� ����� ó�� �� Ÿ��
            selectedEnemyIndex = 0;

            UpdateSelectedEnemy();
        }

        private void Update()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            if (EnemySelectorUI.instance.isTurn)
            {
                // �������� �� ����
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                    {
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1; // ������ ������ ��ȯ
                    }
                    UpdateSelectedEnemy();
                }

                // ���������� �� ����
                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                    {
                        selectedEnemyIndex = 0; // ù ��° ������ ��ȯ
                    }
                    UpdateSelectedEnemy();
                }
            }
            
        }

        private void UpdateSelectedEnemy()
        {
            // ���õ� ���� UI�� �ݿ�
            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);
        }

        public BaseEnemyControl GetSelectedEnemy()
        {
            // ���� ���õ� �� ��ȯ
            if (turnSystem.enemyCharacters.Count > 0)
            {
                return turnSystem.enemyCharacters[selectedEnemyIndex];
            }
            return null;
        }
    }
}

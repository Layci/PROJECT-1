using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemySelection : MonoBehaviour
    {
        public TurnSystem turnSystem;  // 턴 시스템 참조
        public EnemySelectorUI enemySelectorUI; // 선택 UI 관리 스크립트
        private int selectedEnemyIndex = 0;     // 현재 선택된 적의 인덱스

        private void Start()
        {
            EnemySelectorUI.instance.selectorUI.gameObject.SetActive(true);

            // 처음 실행시 처음 적 타겟
            selectedEnemyIndex = 0;

            UpdateSelectedEnemy();
        }

        private void Update()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            if (EnemySelectorUI.instance.isTurn)
            {
                // 왼쪽으로 적 선택
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                    {
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1; // 마지막 적으로 순환
                    }
                    UpdateSelectedEnemy();
                }

                // 오른쪽으로 적 선택
                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                    {
                        selectedEnemyIndex = 0; // 첫 번째 적으로 순환
                    }
                    UpdateSelectedEnemy();
                }
            }
            
        }

        private void UpdateSelectedEnemy()
        {
            // 선택된 적을 UI에 반영
            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);
        }

        public BaseEnemyControl GetSelectedEnemy()
        {
            // 현재 선택된 적 반환
            if (turnSystem.enemyCharacters.Count > 0)
            {
                return turnSystem.enemyCharacters[selectedEnemyIndex];
            }
            return null;
        }
    }
}

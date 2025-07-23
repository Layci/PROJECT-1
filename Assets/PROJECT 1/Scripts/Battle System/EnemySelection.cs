using Project1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project1
{
    public class EnemySelection : MonoBehaviour
    {
        public TurnSystem turnSystem;  // 턴 시스템 참조
        public EnemySelectorUI enemySelectorUI; // 선택 UI 관리 스크립트
        private int selectedEnemyIndex = 0;     // 현재 선택된 적의 인덱스
        public bool isMove = false;

        public static EnemySelection instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

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
            if (EnemySelectorUI.instance.isTurn && !isMove)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1;

                    UpdateSelectedEnemy(); // 중앙 타겟 시각적 갱신
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                        selectedEnemyIndex = 0;

                    UpdateSelectedEnemy(); // 중앙 타겟 시각적 갱신
                }
            }

            /*if (EnemySelectorUI.instance.isTurn && !isMove)
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
            }*/

        }

        public List<int> GetAOETargetIndices(int range = 1)
        {
            List<int> indices = new List<int>();
            int count = turnSystem.enemyCharacters.Count;

            for (int i = -range; i <= range; i++)
            {
                int index = selectedEnemyIndex + i;
                if (index >= 0 && index < count)
                {
                    indices.Add(index);
                }
            }

            return indices;
        }

        public int GetSelectedEnemyIndex()
        {
            return selectedEnemyIndex;
        }

        private void UpdateSelectedEnemy()
        {
            // 선택된 적을 UI에 반영
            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);
        }

        // 범위 내 적 객체(BaseEnemyControl) 리스트 반환 - 공격 처리용
        public List<BaseEnemyControl> GetAOETargets(int range)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();

            int start = Mathf.Max(0, selectedEnemyIndex - range);
            int end = Mathf.Min(turnSystem.enemyCharacters.Count - 1, selectedEnemyIndex + range);

            for (int i = start; i <= end; i++)
            {
                targets.Add(turnSystem.enemyCharacters[i]);
            }

            return targets;
        }
        /*public List<BaseEnemyControl> GetAOETargets(int range = 1)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();
            var allEnemies = turnSystem.enemyCharacters;

            for (int i = -range; i <= range; i++)
            {
                int index = selectedEnemyIndex + i;
                if (index >= 0 && index < allEnemies.Count)
                {
                    targets.Add(allEnemies[index]);
                }
            }

            return targets;
        }*/

        // 범위 내 적 Transform 리스트 반환 - UI 처리용
        public List<Transform> GetAOETargets()
        {
            return GetAOETargets(1).Select(enemy => enemy.transform).ToList();
        }

        /*public BaseEnemyControl GetSelectedEnemy()
        {
            // 현재 선택된 적 반환
            if (turnSystem.enemyCharacters.Count > 0)
            {
                return turnSystem.enemyCharacters[selectedEnemyIndex];
            }
            return null;
        }*/
    }
}

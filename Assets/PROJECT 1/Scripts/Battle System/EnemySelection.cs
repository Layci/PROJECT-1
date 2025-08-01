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
        public bool isPreparingAOEAttack = false;

        // 범위 관련
        //public GameObject aoeSelectorPrefab;
        //public Transform selectorParent; // UI 부모 오브젝트 (캔버스 아래에 위치)
        //private List<GameObject> aoeSelectors = new List<GameObject>();

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
            //EnemySelectorUI.instance.selectorUI.gameObject.SetActive(true);
            if (enemySelectorUI != null)
            {
                enemySelectorUI.SetSelectedEnemy(GetSelectedEnemyTransform());
                enemySelectorUI.ShowSingleTargetUI();
            }

            // 처음 실행시 처음 적 타겟
            selectedEnemyIndex = 0;
            UpdateSelectedEnemy();
        }

        private void Update()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            if (EnemySelectorUI.instance.isTurn && !isMove)
            {
                bool moved = false;

                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0)
                        selectedEnemyIndex = turnSystem.enemyCharacters.Count - 1;
                    moved = true;
                    /*UpdateSelectedEnemy(); // 중앙 타겟 시각적 갱신
                    UpdateAOEIndicators(1); // 여기서 1은 범위*/
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                        selectedEnemyIndex = 0;
                    moved = true;
                    /*UpdateSelectedEnemy(); // 중앙 타겟 시각적 갱신
                    UpdateAOEIndicators(1); // 여기서 1은 범위*/
                }

                if (moved)
                {
                    UpdateSelectedEnemy();
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

        public int GetSelectedEnemyIndex()
        {
            return selectedEnemyIndex;
        }

        public Transform GetSelectedEnemyTransform()
        {
            if (turnSystem.enemyCharacters.Count == 0) return null;
            return turnSystem.enemyCharacters[selectedEnemyIndex].transform;
        }

        /*public void UpdateAOEIndicators(int range)
        {
            // 이전 셀렉터 제거
            foreach (var selector in aoeSelectors)
            {
                Destroy(selector);
            }
            aoeSelectors.Clear();

            int start = Mathf.Max(0, selectedEnemyIndex - range);
            int end = Mathf.Min(turnSystem.enemyCharacters.Count - 1, selectedEnemyIndex + range);

            for (int i = start; i <= end; i++)
            {
                Transform targetTransform = turnSystem.enemyCharacters[i].transform;

                GameObject selector = Instantiate(aoeSelectorPrefab, selectorParent);
                selector.transform.position = Camera.main.WorldToScreenPoint(targetTransform.position + Vector3.up * 2f); // 적 머리 위에 표시
                aoeSelectors.Add(selector);
            }
        }*/

        // 범위 기준 (center + 좌우 range) 적 객체 리스트 반환
        public List<BaseEnemyControl> GetAOETargets(int range = 1)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();
            var allEnemies = turnSystem.enemyCharacters;

            int start = Mathf.Max(0, selectedEnemyIndex - range);
            int end = Mathf.Min(allEnemies.Count - 1, selectedEnemyIndex + range);

            for (int i = start; i <= end; i++)
            {
                targets.Add(allEnemies[i]);
            }

            return targets;
        }

        /*public List<int> GetAOETargetIndices(int range = 1)
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
        }*/

        // EnemySelection 쪽: 외부에서 범위 모드 여부를 주입
        public void UpdateSelectedEnemy(bool isPreparingAOE)
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

            if (isPreparingAOE)
            {
                enemySelectorUI.ShowCurrentAOERange();
            }
            else
            {
                enemySelectorUI.HideAOEUI();
                enemySelectorUI.ShowSingleTargetUI();
            }
        }

        // 오버로드 없이 이걸 쓰도록
        public void UpdateSelectedEnemy()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

            if (isPreparingAOEAttack)
            {
                enemySelectorUI.ShowCurrentAOERange(); // 범위 UI
            }
            else
            {
                enemySelectorUI.HideAOEUI();
                enemySelectorUI.ShowSingleTargetUI(); // 단일 UI
            }
        }

        // 선택된 적 및 UI 갱신
        /*public void UpdateSelectedEnemy()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];
            enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

            if (isPreparingAOEAttack)
            {
                enemySelectorUI.ShowCurrentAOERange(); // 범위 UI
            }
            else
            {
                enemySelectorUI.HideAOEUI();
                enemySelectorUI.ShowSingleTargetUI();
            }
        }*/

        /*public int GetSelectedEnemyIndex()
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
        }*/
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

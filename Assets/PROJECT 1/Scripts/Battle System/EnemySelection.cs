using Project1;
using ProJect1;
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
        public int selectedEnemyIndex = 0;     // 현재 선택된 적의 인덱스
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
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedEnemyIndex++;
                    if (selectedEnemyIndex >= turnSystem.enemyCharacters.Count)
                        selectedEnemyIndex = 0;
                    moved = true;
                }

                if (moved)
                {
                    UpdateSelectedEnemy();
                }
            }
        }

        public void CheckAOESelect()
        {
            // 아군 기본공격이 범위일 경우
            if (enemySelectorUI != null && BaseCharacterControl.instance.normalAttackRange > 0)
            {

            }
        }

        public Transform GetSelectedEnemyTransform()
        {
            if (turnSystem.enemyCharacters.Count == 0) return null;
            return turnSystem.enemyCharacters[selectedEnemyIndex].transform;
        }

        public List<BaseEnemyControl> GetAOETargets(int range = 1)
        {
            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();
            var allEnemies = turnSystem.enemyCharacters;

            // 현재 선택된 적 인덱스
            if (selectedEnemyIndex < 0 || selectedEnemyIndex >= allEnemies.Count)
                return targets;

            int start = Mathf.Max(0, selectedEnemyIndex - range);
            int end = Mathf.Min(allEnemies.Count - 1, selectedEnemyIndex + range);

            for (int i = start; i <= end; i++)
            {
                targets.Add(allEnemies[i]);
            }

            return targets;
        }

        public void UpdateSelectedEnemy()
        {
            if (turnSystem.enemyCharacters.Count == 0) return;

            BaseEnemyControl selectedEnemy = turnSystem.enemyCharacters[selectedEnemyIndex];

            // 현재 캐릭터 가져오기
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseUnit;
            //int range = cur != null ? cur.skillAttackRange : 0;

            if (cur is BaseCharacterControl player)
            {
                if (player.prepareState == AttackPrepareState.Basic)
                {
                    // 기본 공격 → normalAttackRange 사용
                    int normalRange = player.normalAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (normalRange == 0)
                    {
                        // 단일
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        // 범위
                        var targets = GetAOETargets(normalRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
                else if (player.prepareState == AttackPrepareState.Skill)
                {
                    // 스킬 공격 → skillAttackRange 사용
                    int skillRange = player.skillAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (skillRange == 0)
                    {
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        var targets = GetAOETargets(skillRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
            }
            
            //빽업

            /*if (cur is BaseCharacterControl player)
            {
                if (player.prepareState == AttackPrepareState.Basic)
                {
                    // 기본 공격 → normalAttackRange 사용
                    int normalRange = player.normalAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (normalRange == 0)
                    {
                        // 단일
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        // 범위
                        var targets = GetAOETargets(normalRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
                else if (player.prepareState == AttackPrepareState.Skill)
                {
                    // 스킬 공격 → skillAttackRange 사용
                    int skillRange = player.skillAttackRange;

                    enemySelectorUI.SetSelectedEnemy(selectedEnemy.transform);

                    if (skillRange == 0)
                    {
                        enemySelectorUI.ShowSingleTargetUI();
                        enemySelectorUI.HideAOEUI();
                    }
                    else
                    {
                        var targets = GetAOETargets(skillRange);
                        enemySelectorUI.ShowAOETargets(targets.Select(e => e.transform).ToList());
                        enemySelectorUI.HideSingleTargetUI();
                    }
                }
            }*/
        }
    }
}

using Project1;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Project1
{
    public class HoshiPlayerControl : BaseCharacterControl
    {
        private TurnSystem turnSystem;

        private void Start()
        {
            turnSystem = FindObjectOfType<TurnSystem>();
        }

        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();

            // 복구용 예전 코드
            void demi()
            {
                /*HandleAttackModeInput(); // Q↔E 전환 공통 처리

            // 공격 가능한 상태가 아니면 리턴
            if (!CanAttack())
                return;

            // ----------------- 일반 공격 모드 진입 -----------------
            if (Input.GetKeyDown(KeyCode.Q))
            {
                prepareState = AttackPrepareState.Basic;
                skillAttack = false;
                EnemySelectorUI.instance.ShowSingleTargetUI();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (SkillPointManager.instance.curSkillPoint > 0)
                {
                    prepareState = AttackPrepareState.Skill;
                    EnemySelectorUI.instance.ShowAOETargets(GetAOETargets().Select(e => e.transform).ToList());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                // 공격 확정 (Enter 키 예시)
                if (prepareState == AttackPrepareState.Basic)
                {
                    prepareState = AttackPrepareState.None;
                    currentState = PlayerState.MovingToAttack;
                    skillAttack = false;
                    isPreparingSingleAttack = false;
                    EnemySelectorUI.instance.HideSingleTargetUI();
                }
                else if (prepareState == AttackPrepareState.Skill)
                {
                    prepareState = AttackPrepareState.None;
                    currentState = PlayerState.MovingToAttack;

                    SkillPointManager.instance.UseSkillPoint();
                    EnemySelectorUI.instance.HideAOEUI();
                }
            }*/

                /*if (!isPreparingSingleAttack && !isPreparingAOEAttack && Input.GetKeyDown(KeyCode.Q))
                {
                    isPreparingSingleAttack = true;
                    skillAttack = false;

                    EnemySelection.instance.isPreparingAOEAttack = false;
                    EnemySelection.instance.UpdateSelectedEnemy();

                    EnemySelectorUI.instance.ShowSingleTargetUI();
                }
                // ----------------- 일반 공격 확정 -----------------
                else if (isPreparingSingleAttack && Input.GetKeyDown(KeyCode.Q))
                {
                    skillAttack = false;
                    isPreparingSingleAttack = false;
                    SkillPointManager.instance.SkillPointUp();
                    currentState = PlayerState.MovingToAttack;

                    EnemySelectorUI.instance.HideSingleTargetUI();

                    EnemySelection.instance.isPreparingAOEAttack = false;
                    EnemySelection.instance.UpdateSelectedEnemy();
                }
                // ----------------- 일반 → 범위 전환 -----------------
                else if (isPreparingSingleAttack && Input.GetKeyDown(KeyCode.E))
                {
                    isPreparingSingleAttack = false;
                    isPreparingAOEAttack = true;
                    skillAttack = false;

                    EnemySelection.instance.isPreparingAOEAttack = true;
                    EnemySelection.instance.UpdateSelectedEnemy();

                    var targets = GetAOETargetsAroundSelected();
                    var targetTransforms = targets.Select(t => t.transform).ToList();
                    EnemySelectorUI.instance.ShowAOETargets(targetTransforms);
                    EnemySelectorUI.instance.HideSingleTargetUI();
                }

                // ----------------- 범위 공격 모드 진입 -----------------
                else if (!isPreparingAOEAttack && !isPreparingSingleAttack && Input.GetKeyDown(KeyCode.E))
                {
                    if (SkillPointManager.instance.curSkillPoint > 0)
                    {
                        isPreparingAOEAttack = true;
                        skillAttack = false;

                        EnemySelection.instance.isPreparingAOEAttack = true;
                        EnemySelection.instance.UpdateSelectedEnemy();

                        var targets = GetAOETargetsAroundSelected();
                        var targetTransforms = targets.Select(t => t.transform).ToList();
                        EnemySelectorUI.instance.ShowAOETargets(targetTransforms);
                    }
                }
                // ----------------- 범위 공격 확정 -----------------
                else if (isPreparingAOEAttack && Input.GetKeyDown(KeyCode.E))
                {
                    skillAttack = true;
                    isPreparingAOEAttack = false;

                    EnemySelection.instance.isPreparingAOEAttack = false;
                    EnemySelection.instance.UpdateSelectedEnemy();

                    SkillPointManager.instance.UseSkillPoint();
                    currentState = PlayerState.MovingToAttack;

                    EnemySelectorUI.instance.HideAOEUI();
                }
                // ----------------- 범위 → 단일 전환 -----------------
                else if (isPreparingAOEAttack && Input.GetKeyDown(KeyCode.Q))
                {
                    isPreparingAOEAttack = false;
                    isPreparingSingleAttack = true;
                    skillAttack = false;

                    EnemySelection.instance.isPreparingAOEAttack = false;
                    EnemySelection.instance.UpdateSelectedEnemy();

                    EnemySelectorUI.instance.HideAOEUI();
                    EnemySelectorUI.instance.ShowSingleTargetUI();
                }*/
            }

        }

        // 복구용 예전 코드2
        void demi2()
        {
            /*protected override void HandleAttackInput()
        {
            if (currentState != PlayerState.Idle || EnemySelection.instance.isMove) return;

            // ----------------- 일반 공격 모드 -----------------
            if (!isPreparingSingleAttack && !isPreparingAOEAttack && Input.GetKeyDown(KeyCode.Q))
            {
                isPreparingSingleAttack = true;
                EnemySelectorUI.instance.ShowSingleTargetUI();
            }
            else if (isPreparingSingleAttack && Input.GetKeyDown(KeyCode.Q))
            {
                skillAttack = false;
                isPreparingSingleAttack = false;
                SkillPointManager.instance.SkillPointUp();
                currentState = PlayerState.MovingToAttack;
                EnemySelectorUI.instance.HideSingleTargetUI();
            }
            // Q 누른 상태에서 E 누르면 범위로 전환
            else if (isPreparingSingleAttack && Input.GetKeyDown(KeyCode.E))
            {
                isPreparingSingleAttack = false;
                isPreparingAOEAttack = true;

                var targets = GetAOETargetsAroundSelected();
                var targetTransforms = targets.Select(t => t.transform).ToList();
                EnemySelection.instance.UpdateSelectedEnemy(isPreparingAOEAttack);
                EnemySelectorUI.instance.ShowAOETargets(targetTransforms);
                EnemySelectorUI.instance.HideSingleTargetUI();
            }

            // ----------------- 범위 공격 모드 -----------------
            else if (!isPreparingAOEAttack && !isPreparingSingleAttack && Input.GetKeyDown(KeyCode.E))
            {
                if (SkillPointManager.instance.curSkillPoint > 0)
                {
                    isPreparingAOEAttack = true;

                    var targets = GetAOETargetsAroundSelected();
                    var targetTransforms = targets.Select(t => t.transform).ToList();
                    EnemySelection.instance.UpdateSelectedEnemy(isPreparingAOEAttack);
                    EnemySelectorUI.instance.ShowAOETargets(targetTransforms);
                }
            }
            else if (isPreparingAOEAttack && Input.GetKeyDown(KeyCode.E))
            {
                skillAttack = true;
                isPreparingAOEAttack = false;
                SkillPointManager.instance.UseSkillPoint();
                currentState = PlayerState.MovingToAttack;

                EnemySelectorUI.instance.HideAOEUI();
            }
            // E 누른 상태에서 Q 누르면 단일로 전환
            else if (isPreparingAOEAttack && Input.GetKeyDown(KeyCode.Q))
            {
                isPreparingAOEAttack = false;
                isPreparingSingleAttack = true;

                EnemySelectorUI.instance.HideAOEUI();
                EnemySelectorUI.instance.ShowSingleTargetUI();
            }
        }*/

            /*protected override void HandleAttackInput()
            {
                if (currentState == PlayerState.Idle)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        skillAttack = false;
                        StartMove();
                        SkillPointManager.instance.SkillPointUp();
                    }
                    else if (!isPreparingAOEAttack && Input.GetKeyDown(KeyCode.E))
                    {
                        if (SkillPointManager.instance.curSkillPoint > 0)
                        {
                            isPreparingAOEAttack = true;

                            var targets = GetAOETargetsAroundSelected();
                            var targetTransforms = targets.Select(t => t.transform).ToList();

                            EnemySelectorUI.instance.HighlightEnemies(targetTransforms);
                            EnemySelectorUI.instance.ShowAOETargets(targetTransforms);
                        }
                    }
                    else if (isPreparingAOEAttack && Input.GetKeyDown(KeyCode.E))
                    {
                        skillAttack = true;
                        isPreparingAOEAttack = false;
                        SkillPointManager.instance.UseSkillPoint();

                        EnemySelectorUI.instance.HideAOEUI();
                        currentState = PlayerState.MovingToAttack;
                    }
                }
            }*/
        }


        /*private List<BaseEnemyControl> GetAOETargetsAroundSelected()
        {
            int selectedIndex = EnemySelection.instance.GetSelectedEnemyIndex();
            List<BaseEnemyControl> allEnemies = TurnSystem.instance.enemyCharacters;

            List<BaseEnemyControl> targets = new List<BaseEnemyControl>();

            for (int i = selectedIndex - 1; i <= selectedIndex + 1; i++)
            {
                if (i >= 0 && i < allEnemies.Count)
                {
                    targets.Add(allEnemies[i]);
                }
            }

            return targets;
        }

        private void StartMove()
        {
            currentState = PlayerState.MovingToAttack;
        }*/
    }
}

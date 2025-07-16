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
                EnemySelectorUI.instance.HighlightEnemies(targetTransforms);
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
                    EnemySelectorUI.instance.HighlightEnemies(targetTransforms);
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
        }

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

        private List<BaseEnemyControl> GetAOETargetsAroundSelected()
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
        }
    }
}

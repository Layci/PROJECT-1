using Project1;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Project1
{
    public class HoshiPlayerControl : BaseCharacterControl
    {
        protected override void HandleAttackInput()
        {
            base.HandleAttackInput();
        }

        // ������ ���� �ڵ�2
        void demi2()
        {
            /*protected override void HandleAttackInput()
        {
            if (currentState != PlayerState.Idle || EnemySelection.instance.isMove) return;

            // ----------------- �Ϲ� ���� ��� -----------------
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
            // Q ���� ���¿��� E ������ ������ ��ȯ
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

            // ----------------- ���� ���� ��� -----------------
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
            // E ���� ���¿��� Q ������ ���Ϸ� ��ȯ
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
    }
}

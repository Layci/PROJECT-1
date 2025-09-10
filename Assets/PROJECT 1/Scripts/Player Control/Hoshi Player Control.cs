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

            // ������ ���� �ڵ�
            void demi()
            {
                /*HandleAttackModeInput(); // Q��E ��ȯ ���� ó��

            // ���� ������ ���°� �ƴϸ� ����
            if (!CanAttack())
                return;

            // ----------------- �Ϲ� ���� ��� ���� -----------------
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
                // ���� Ȯ�� (Enter Ű ����)
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
                // ----------------- �Ϲ� ���� Ȯ�� -----------------
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
                // ----------------- �Ϲ� �� ���� ��ȯ -----------------
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

                // ----------------- ���� ���� ��� ���� -----------------
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
                // ----------------- ���� ���� Ȯ�� -----------------
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
                // ----------------- ���� �� ���� ��ȯ -----------------
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

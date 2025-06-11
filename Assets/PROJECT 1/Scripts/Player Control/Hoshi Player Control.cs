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
            if (currentState == PlayerState.Idle)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    skillAttack = false;
                    StartMove();
                    SkillPointManager.instance.SkillPointUp();
                }
                else if (SkillPointManager.instance.curSkillPoint > 0 && Input.GetKeyDown(KeyCode.E))
                {
                    skillAttack = true;
                    StartMove();
                    SkillPointManager.instance.UseSkillPoint();
                }
                if (!isPreparingAOEAttack && Input.GetKeyDown(KeyCode.E))
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
        }

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

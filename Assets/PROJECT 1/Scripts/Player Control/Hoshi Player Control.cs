using Project1;
using System;
using UnityEngine;

namespace Project1
{
    public class HoshiPlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform;
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
                    StartMove(enemyTransform);
                    SkillPointManager.instance.SkillPointUp();
                    EndPlayerTurn(); // ���� �� �� ����
                }
                else if (SkillPointManager.instance.curSkillPoint > 0 && Input.GetKeyDown(KeyCode.E))
                {
                    skillAttack = true;
                    StartMove(enemyTransform);
                    SkillPointManager.instance.UseSkillPoint();
                    EndPlayerTurn(); // ��ų ��� �� �� ����
                }
            }
        }

        public override void WaitForInput()
        {
            HandleAttackInput();
        }

        private void StartMove(Transform enemyTransform)
        {
            if (enemyTransform != null)
            {
                enemy = enemyTransform;
                currentState = PlayerState.MovingToAttack;
            }
        }

        private void EndPlayerTurn()
        {
            if (turnSystem != null)
            {
                turnSystem.EndTurn(); // �� ����
            }
        }
    }
}

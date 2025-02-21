using Project1;
using System;
using UnityEngine;

namespace Project1
{
    public class FayePlayerControl : BaseCharacterControl
    {
        private TurnSystem turnSystem;

        // 공격력 버프
        public int powerBuff = 0;
        public int maxPowerBuff = 2;

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
            }
        }

        private void StartMove()
        {
            currentState = PlayerState.MovingToAttack;
        }
    }
}

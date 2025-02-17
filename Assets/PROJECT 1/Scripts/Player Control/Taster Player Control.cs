using Project1;
using System;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
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
                    StartBlock();
                    SkillPointManager.instance.UseSkillPoint();
                }
            }
        }

        private void StartMove()    
        {
            currentState = PlayerState.MovingToAttack;
        }

        private void StartBlock()
        {
            isBlock = true;
            startBlocking = true;
            currentState = PlayerState.Blocking;
        }
    }
}

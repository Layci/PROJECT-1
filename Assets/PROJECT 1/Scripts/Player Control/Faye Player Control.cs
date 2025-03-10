using Project1;
using System;
using UnityEngine;

namespace Project1
{
    public class FayePlayerControl : BaseCharacterControl
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
                Buff FayeAttackBuff = null;
                switch (buffPower)
                {
                    case 1:
                        FayeAttackBuff = new Buff("Faye공격력 증가", 3, 0.2f, 0);
                        break;
                    case 2:
                        FayeAttackBuff = new Buff("Faye공격력 증가", 3, 0.5f, 0);
                        break;
                    case 3:
                        FayeAttackBuff = new Buff("Faye공격력 증가", 3, 0.8f, 0);
                        break;
                }

                if (FayeAttackBuff != null)
                {
                    AddBuff(FayeAttackBuff);
                }

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

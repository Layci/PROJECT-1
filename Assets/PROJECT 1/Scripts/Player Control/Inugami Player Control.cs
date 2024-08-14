using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class InugamiPlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform;

        public override void TakeTurn()  // override Ű���� ���
        {
            base.TakeTurn(); // BaseCharacterControl�� TakeTurn�� ȣ��
        }

        protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StartMove(enemyTransform);
                    skillAttack = false;
                    SkillPointManager.instance.SkillPointUp();
                }
                if (SkillPointManager.instance.curSkillPoint > 0)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartMove(enemyTransform);
                        skillAttack = true;
                        SkillPointManager.instance.UseSkillPoint();
                    }
                }
            }
        }
    }
}

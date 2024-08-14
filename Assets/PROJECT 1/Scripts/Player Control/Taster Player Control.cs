using ProJect1;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
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
using ProJect1;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform;

        public override void TakeTurn()  // override 키워드 사용
        {
            base.TakeTurn(); // BaseCharacterControl의 TakeTurn을 호출
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
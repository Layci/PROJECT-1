using ProJect1;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform; // 적의 위치

        protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle) // 플레이어가 대기 상태일 경우에만 공격 가능
            {
                if (Input.GetKeyDown(KeyCode.Q)) // 공격 키
                {
                    StartMove(enemyTransform);
                    skillAttack = false;
                    SkillPointManager.instance.SkillPointUp();
                }
                if (SkillPointManager.instance.curSkillPoint > 0) // 스킬 포인트가 1이상일 경우에만 사용가능 
                {
                    if (Input.GetKeyDown(KeyCode.E)) // 스킬 키
                    {
                        StartMove(enemyTransform); // 스킬 사용도 이동 후 공격
                        skillAttack = true;
                        SkillPointManager.instance.UseSkillPoint();
                    }
                }
            }
        }
    }
}
using UnityEngine;

namespace Project1
{
    public class PlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform; // 적의 위치

        protected override void HandleAttackInput()
        {
            if (Input.GetKeyDown(KeyCode.Q)) // 공격 키
            {
                StartMove(enemyTransform);
            }
            if (Input.GetKeyDown(KeyCode.E)) // 스킬 키
            {
                StartMove(enemyTransform); // 스킬 사용도 이동 후 공격
            }
        }

        protected override void MoveToAttack()
        {
            base.MoveToAttack();
            // 필요시 MoveToAttack을 여기서 추가로 수정 가능
        }
    }
}

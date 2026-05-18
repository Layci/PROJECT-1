using Project1;
using UnityEngine;

namespace Project1
{
    public class PlayerAttackBehaviour : StateMachineBehaviour
    {
        // 애니메이션이 시작될 때 호출됩니다.
        /*public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 있는 부모 객체에서 BaseCharacterControl 컴포넌트를 가져옵니다.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // 공격이 시작됨을 알립니다.
                characterControl.startAttacking = true;
            }
        }*/

        // 애니메이션이 종료될 때 호출됩니다.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 있는 부모 객체에서 BaseCharacterControl 컴포넌트를 가져옵니다.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // 원거리 공격 상태였다면 즉시 턴 종료, 아니라면 복귀 상태로 전환
                if (characterControl.currentState == PlayerState.RangedAttacking)
                {
                    characterControl.EndTurnManually();
                }
                else
                {
                    characterControl.currentState = PlayerState.Returning;
                }
                
                characterControl.startAttacking = false;
            }
        }
    }
}

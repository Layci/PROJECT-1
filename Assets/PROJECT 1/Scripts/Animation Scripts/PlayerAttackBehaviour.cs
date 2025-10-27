using Project1;
using UnityEngine;

namespace Project1
{
    public class PlayerAttackBehaviour : StateMachineBehaviour
    {
        // 애니메이터 상태 진입 시 호출됩니다.
        /*public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseCharacterControl 컴포넌트를 가져옵니다.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // 공격이 시작됨을 알립니다.
                characterControl.startAttacking = true;
            }
        }*/

        // 애니메이터 상태 종료 시 호출됩니다.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseCharacterControl 컴포넌트를 가져옵니다.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // 공격이 끝났음을 알립니다.
                characterControl.currentState = PlayerState.Returning;
                characterControl.startAttacking = false;
            }
        }
    }
}

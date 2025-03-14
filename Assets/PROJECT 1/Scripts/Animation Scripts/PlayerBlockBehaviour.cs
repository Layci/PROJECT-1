using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PlayerBlockBehaviour : StateMachineBehaviour
    {
        // 애니메이터 상태 진입 시 호출됩니다.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseCharacterControl 컴포넌트를 가져옵니다.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // 공격이 시작됨을 알립니다.
                /*characterControl.startBlocking = true;
                characterControl.isBlock = true;*/
                //characterControl.damageReduction = 0.7f;
            }
        }

        // 애니메이터 상태 종료 시 호출됩니다.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseCharacterControl 컴포넌트를 가져옵니다.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // 공격이 끝났음을 알립니다.
                /*characterControl.currentState = PlayerState.Idle;
                characterControl.startBlocking = false;
                characterControl.isBlock = false;
                characterControl.damageReduction = 1f;*/
            }
        }
    }
}

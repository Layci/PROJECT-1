using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class EnemyAttackBehaviour : StateMachineBehaviour
    {
        // 애니메이터 상태 진입 시 호출됩니다.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseEnemyControl 컴포넌트를 가져옵니다.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                // 공격이 시작됨을 알립니다.
                enemy.startAttacking = true;
            }
        }

        // 애니메이터 상태 종료 시 호출됩니다.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseEnemyControl 컴포넌트를 가져옵니다.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                // 공격이 끝났음을 알립니다.
                enemy.currentState = EnemyState.Returning;
            }
        }
    }
}

using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyAttackBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseEnemyControl 컴포넌트를 가져옵니다.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                enemy.startAttacking = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator가 속한 부모 객체에서 BaseEnemyControl 컴포넌트를 가져옵니다.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                enemy.currentState = EnemyState.Returning;
            }
        }
    }
}

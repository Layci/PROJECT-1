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
            // Animator�� ���� �θ� ��ü���� BaseEnemyControl ������Ʈ�� �����ɴϴ�.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                enemy.startAttacking = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseEnemyControl ������Ʈ�� �����ɴϴ�.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                enemy.currentState = EnemyState.Returning;
            }
        }
    }
}

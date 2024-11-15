using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class EnemyAttackBehaviour : StateMachineBehaviour
    {
        // �ִϸ����� ���� ���� �� ȣ��˴ϴ�.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseEnemyControl ������Ʈ�� �����ɴϴ�.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                // ������ ���۵��� �˸��ϴ�.
                enemy.startAttacking = true;
            }
        }

        // �ִϸ����� ���� ���� �� ȣ��˴ϴ�.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseEnemyControl ������Ʈ�� �����ɴϴ�.
            BaseEnemyControl enemy = animator.GetComponentInParent<BaseEnemyControl>();
            if (enemy != null)
            {
                // ������ �������� �˸��ϴ�.
                enemy.currentState = EnemyState.Returning;
            }
        }
    }
}

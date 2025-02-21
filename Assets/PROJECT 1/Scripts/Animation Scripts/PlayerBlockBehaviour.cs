using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PlayerBlockBehaviour : StateMachineBehaviour
    {
        // �ִϸ����� ���� ���� �� ȣ��˴ϴ�.
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseCharacterControl ������Ʈ�� �����ɴϴ�.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // ������ ���۵��� �˸��ϴ�.
                /*characterControl.startBlocking = true;
                characterControl.isBlock = true;*/
                characterControl.damageReduction = 0.7f;
            }
        }

        // �ִϸ����� ���� ���� �� ȣ��˴ϴ�.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseCharacterControl ������Ʈ�� �����ɴϴ�.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // ������ �������� �˸��ϴ�.
                /*characterControl.currentState = PlayerState.Idle;
                characterControl.startBlocking = false;
                characterControl.isBlock = false;
                characterControl.damageReduction = 1f;*/
            }
        }
    }
}

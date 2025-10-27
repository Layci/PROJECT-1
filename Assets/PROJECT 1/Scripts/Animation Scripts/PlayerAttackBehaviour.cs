using Project1;
using UnityEngine;

namespace Project1
{
    public class PlayerAttackBehaviour : StateMachineBehaviour
    {
        // �ִϸ����� ���� ���� �� ȣ��˴ϴ�.
        /*public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseCharacterControl ������Ʈ�� �����ɴϴ�.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // ������ ���۵��� �˸��ϴ�.
                characterControl.startAttacking = true;
            }
        }*/

        // �ִϸ����� ���� ���� �� ȣ��˴ϴ�.
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Animator�� ���� �θ� ��ü���� BaseCharacterControl ������Ʈ�� �����ɴϴ�.
            BaseCharacterControl characterControl = animator.GetComponentInParent<BaseCharacterControl>();
            if (characterControl != null)
            {
                // ������ �������� �˸��ϴ�.
                characterControl.currentState = PlayerState.Returning;
                characterControl.startAttacking = false;
            }
        }
    }
}

using UnityEngine;

namespace Project1
{
    public class PlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform; // ���� ��ġ

        protected override void HandleAttackInput()
        {
            if (Input.GetKeyDown(KeyCode.Q)) // ���� Ű
            {
                StartMove(enemyTransform);
            }
            if (Input.GetKeyDown(KeyCode.E)) // ��ų Ű
            {
                StartMove(enemyTransform); // ��ų ��뵵 �̵� �� ����
            }
        }

        protected override void MoveToAttack()
        {
            base.MoveToAttack();
            // �ʿ�� MoveToAttack�� ���⼭ �߰��� ���� ����
        }
    }
}

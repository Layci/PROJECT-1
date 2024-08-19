using ProJect1;
using UnityEngine;

namespace Project1
{
    public class TasterPlayerControl : BaseCharacterControl
    {
        public Transform enemyTransform; // ���� ��ġ

        protected override void HandleAttackInput()
        {
            if (currentState == PlayerState.Idle) // �÷��̾ ��� ������ ��쿡�� ���� ����
            {
                if (Input.GetKeyDown(KeyCode.Q)) // ���� Ű
                {
                    StartMove(enemyTransform);
                    skillAttack = false;
                    SkillPointManager.instance.SkillPointUp();
                }
                if (SkillPointManager.instance.curSkillPoint > 0) // ��ų ����Ʈ�� 1�̻��� ��쿡�� ��밡�� 
                {
                    if (Input.GetKeyDown(KeyCode.E)) // ��ų Ű
                    {
                        StartMove(enemyTransform); // ��ų ��뵵 �̵� �� ����
                        skillAttack = true;
                        SkillPointManager.instance.UseSkillPoint();
                    }
                }
            }
        }
    }
}
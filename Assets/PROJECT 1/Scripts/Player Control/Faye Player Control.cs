using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class FayePlayerControl : BaseCharacterControl
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

        protected override void MoveToAttack()
        {
            base.MoveToAttack();
            // �ʿ�� MoveToAttack�� ���⼭ �߰��� ���� ����
        }
    }
}
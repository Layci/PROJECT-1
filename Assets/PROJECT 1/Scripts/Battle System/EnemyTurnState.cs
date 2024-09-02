using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project1
{
    public class EnemyTurnState : GameState
    {
        public override void EnterState(TurnSystem turnSystem)
        {
            Debug.Log("���� �� ����");
            PerformEnemyAttack(turnSystem);
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            // ���� ������ �Ϸ��ϸ� �÷��̾� ������ ��ȯ
            turnSystem.ChangeState(new PlayerTurnState());
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("���� �� ����");
        }

        private void PerformEnemyAttack(TurnSystem turnSystem)
        {
            // ���� ���� ���� �߰�
            Debug.Log("���� �÷��̾ �����մϴ�!");

            // ���⼭ �÷��̾�� �������� ���ϴ� ���� ����
            // ��: PlayerHealth.Instance.TakeDamage(attackDamage);
        }
    }
}

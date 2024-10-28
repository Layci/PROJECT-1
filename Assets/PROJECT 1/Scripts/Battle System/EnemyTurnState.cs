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
            turnSystem.PerformEnemyAttack();
            turnSystem.ChangeState(new PlayerTurnState()); // �� ������ ������ �÷��̾� ������ ��ȯ
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            // �� �Ͽ����� UpdateState�� �ʿ����� ���� �� �ֽ��ϴ�.
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("���� �� ����");
        }
    }
}

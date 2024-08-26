using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyTurnState : GameState
    {
        public override void EnterState(TurnSystem turnSystem)
        {
            Debug.Log("���� �� ����");
            // ���� �ൿ�� ����
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            // ���� �ڵ����� ������ ����
            Debug.Log("���� ������ �����մϴ�.");
            turnSystem.ChangeState(new PlayerTurnState());  // �� ���� ������ �÷��̾� ������ ��ȯ
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("���� �� ����");
        }
    }
}

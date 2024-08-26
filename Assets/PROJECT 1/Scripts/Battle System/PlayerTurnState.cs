using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PlayerTurnState : GameState
    {
        public override void EnterState(TurnSystem turnSystem)
        {
            Debug.Log("�÷��̾��� �� ����");
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            if (Input.GetKeyDown(KeyCode.Q))  // �÷��̾ ���� ����� ���� ���
            {
                Debug.Log("�÷��̾ ������ �����մϴ�.");
                turnSystem.ChangeState(new EnemyTurnState());  // �÷��̾� ���� ������ �� ������ ��ȯ
            }
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("�÷��̾��� �� ����");
        }
    }
}

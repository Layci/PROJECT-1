using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class TurnSystem : MonoBehaviour
    {
        private GameState currentState;

        private void Start()
        {
            ChangeState(new PlayerTurnState());  // ���� ���� �� ù ���¸� ����
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState(this);  // �� ������ ���� ������Ʈ
            }
        }

        public void ChangeState(GameState newState)
        {
            if (currentState != null)
            {
                currentState.ExitState(this);   // ���� ���� ����
            }

            currentState = newState;
            currentState.EnterState(this);      // ���ο� ���·� ����
        }
    }
}

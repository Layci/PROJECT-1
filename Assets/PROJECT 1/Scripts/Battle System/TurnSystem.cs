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
            ChangeState(new PlayerTurnState());  // 게임 시작 시 첫 상태를 설정
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState(this);  // 매 프레임 상태 업데이트
            }
        }

        public void ChangeState(GameState newState)
        {
            if (currentState != null)
            {
                currentState.ExitState(this);   // 현재 상태 종료
            }

            currentState = newState;
            currentState.EnterState(this);      // 새로운 상태로 진입
        }
    }
}

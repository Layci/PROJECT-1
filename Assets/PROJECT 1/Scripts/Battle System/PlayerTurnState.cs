/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project1
{
    public class PlayerTurnState : GameState
    {
        public override void EnterState(TurnSystem turnSystem)
        {
            Debug.Log("플레이어의 턴 시작");
            turnSystem.StartPlayerTurn();
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // 플레이어가 공격 명령을 내릴 경우
            {
                Debug.Log("플레이어가 공격을 시작합니다.");
                turnSystem.ChangeState(new EnemyTurnState()); // 플레이어 턴이 끝나면 적 턴으로 전환
            }
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("플레이어의 턴 종료");
        }
    }
}
*/
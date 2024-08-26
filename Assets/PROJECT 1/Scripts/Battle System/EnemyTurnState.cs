using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyTurnState : GameState
    {
        public override void EnterState(TurnSystem turnSystem)
        {
            Debug.Log("적의 턴 시작");
            // 적이 행동을 결정
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            // 적이 자동으로 공격을 수행
            Debug.Log("적이 공격을 수행합니다.");
            turnSystem.ChangeState(new PlayerTurnState());  // 적 턴이 끝나면 플레이어 턴으로 전환
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("적의 턴 종료");
        }
    }
}

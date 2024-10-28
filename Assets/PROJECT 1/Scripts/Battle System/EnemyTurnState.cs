using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project1
{
    public class EnemyTurnState : GameState
    {
        public override void EnterState(TurnSystem turnSystem)
        {
            Debug.Log("적의 턴 시작");
            turnSystem.PerformEnemyAttack();
            turnSystem.ChangeState(new PlayerTurnState()); // 적 공격이 끝나면 플레이어 턴으로 전환
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            // 적 턴에서는 UpdateState가 필요하지 않을 수 있습니다.
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("적의 턴 종료");
        }
    }
}

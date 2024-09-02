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
            PerformEnemyAttack(turnSystem);
        }

        public override void UpdateState(TurnSystem turnSystem)
        {
            // 적이 공격을 완료하면 플레이어 턴으로 전환
            turnSystem.ChangeState(new PlayerTurnState());
        }

        public override void ExitState(TurnSystem turnSystem)
        {
            Debug.Log("적의 턴 종료");
        }

        private void PerformEnemyAttack(TurnSystem turnSystem)
        {
            // 적의 공격 로직 추가
            Debug.Log("적이 플레이어를 공격합니다!");

            // 여기서 플레이어에게 데미지를 가하는 로직 구현
            // 예: PlayerHealth.Instance.TakeDamage(attackDamage);
        }
    }
}

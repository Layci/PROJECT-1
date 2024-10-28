using System.Collections;
using System.Collections.Generic;
using System.Linq;  // 리스트 정렬을 위해 추가
using UnityEngine;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public List<BaseCharacterControl> playerUnits; // 플레이어 유닛 리스트
        public List<BaseEnemyControl> enemyUnits;      // 적 유닛 리스트
        private List<MonoBehaviour> turnOrder;         // 턴 순서에 따라 유닛 저장
        private int currentTurnIndex = 0;              // 현재 턴을 진행 중인 유닛 인덱스

        public GameState currentState;

        private void Start()
        {
            SetupTurnOrder();  // 턴 순서를 초기화
            StartNextTurn();   // 첫 번째 턴 시작
        }

        // 턴 순서를 설정 (속도에 따라)
        private void SetupTurnOrder()
        {
            // 모든 유닛(플레이어와 적)을 하나의 리스트로 합치고 속도에 따라 정렬
            turnOrder = new List<MonoBehaviour>();
            turnOrder.AddRange(playerUnits);
            turnOrder.AddRange(enemyUnits);
            turnOrder = turnOrder.OrderByDescending(unit =>
                (unit is BaseCharacterControl ? ((BaseCharacterControl)unit).unitSpeed : ((BaseEnemyControl)unit).unitSpeed)
            ).ToList();
        }

        // 턴을 종료하고 다음 턴 시작
        public void EndTurn()
        {
            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
            StartNextTurn();
        }

        // 현재 턴 유닛에 따라 적절한 턴 시작
        private void StartNextTurn()
        {
            if (turnOrder[currentTurnIndex] is BaseCharacterControl playerUnit)
            {
                StartPlayerTurn(playerUnit);  // 플레이어 턴 시작
            }
            else if (turnOrder[currentTurnIndex] is BaseEnemyControl enemyUnit)
            {
                StartEnemyTurn(enemyUnit);    // 적 턴 시작
            }
        }

        // 플레이어 턴 시작 (단일 플레이어 유닛)
        private void StartPlayerTurn(BaseCharacterControl playerUnit)
        {
            Debug.Log($"{playerUnit.name}의 턴입니다.");
            playerUnit.WaitForInput();  // 플레이어 입력을 기다림
        }

        // 적 턴 시작 (단일 적 유닛)
        private void StartEnemyTurn(BaseEnemyControl enemyUnit)
        {
            Debug.Log($"{enemyUnit.name}의 턴입니다.");
            enemyUnit.StartAttack();   // 적은 자동으로 공격을 수행
            EndTurn();  // 적의 공격 후 즉시 턴 종료
        }

        // 플레이어 턴을 시작하는 함수 (기존 로직에 맞게)
        public void StartPlayerTurn()
        {
            if (currentState != null)
            {
                currentState.EnterState(this);
            }
        }

        public void PerformEnemyAttack()
        {
            // 적의 공격 로직을 처리하는 함수
            foreach (var enemy in enemyUnits)
            {
                enemy.StartAttack();
            }
        }

        public void ChangeState(GameState newState)
        {
            currentState.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }
    }
}

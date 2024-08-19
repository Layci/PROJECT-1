/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public enum GameTurnState
    {
        Waiting,   // 다른 유닛의 턴이 진행 중인 상태
        Active,    // 현재 턴이 진행 중인 상태
        Completed  // 현재 턴이 종료된 상태
    }

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        private IUnit currentUnit;
        private int currentUnitIndex = 0;
        private List<IUnit> allUnits = new List<IUnit>();

        public GameTurnState gameTurnState = GameTurnState.Waiting;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // 모든 유닛을 리스트에 추가하고 속도 기준으로 정렬
            allUnits.AddRange(FindObjectsOfType<BaseCharacterControl>());
            allUnits.AddRange(FindObjectsOfType<BaseEnemyControl>());
            allUnits.Sort((a, b) => b.UnitSpeed.CompareTo(a.UnitSpeed));

            // 첫 번째 유닛의 턴 시작
            StartNextTurn();
        }

        public void StartNextTurn()
        {
            if (allUnits.Count == 0) return;

            // 모든 유닛의 턴 상태를 대기 상태로 설정
            gameTurnState = GameTurnState.Waiting;

            currentUnit = allUnits[currentUnitIndex];

            // 현재 유닛의 턴을 활성화
            gameTurnState = GameTurnState.Active;
            currentUnit.TakeTurn();
        }

        public void OnUnitTurnCompleted()
        {
            // 현재 유닛의 턴 종료
            gameTurnState = GameTurnState.Completed;

            // 다음 유닛의 턴으로 전환
            currentUnitIndex = (currentUnitIndex + 1) % allUnits.Count;
            StartCoroutine(WaitAndStartNextTurn());
        }

        private IEnumerator WaitAndStartNextTurn()
        {
            yield return new WaitForSeconds(0.5f); // 0.5초 대기
            StartNextTurn();
        }

        public bool IsCurrentUnit(IUnit unit)
        {
            return unit == currentUnit;
        }
    }
}
*/
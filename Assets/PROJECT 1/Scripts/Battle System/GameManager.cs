using UnityEngine;
using System.Collections.Generic;

namespace Project1
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private IUnit currentUnit;
        private int currentUnitIndex = 0;
        private List<IUnit> allUnits = new List<IUnit>(); // 유닛 리스트

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
            // 모든 유닛(플레이어 및 적)을 리스트에 추가
            allUnits.AddRange(FindObjectsOfType<BaseCharacterControl>());
            allUnits.AddRange(FindObjectsOfType<BaseEnemyControl>());

            // 속도 기준으로 정렬
            allUnits.Sort((a, b) => b.UnitSpeed.CompareTo(a.UnitSpeed));

            // 첫 번째 유닛의 턴 시작
            StartNextTurn();
        }

        public void StartNextTurn()
        {
            if (allUnits.Count == 0) return;

            currentUnit = allUnits[currentUnitIndex];
            currentUnit.TakeTurn();
        }

        public void OnUnitTurnCompleted()
        {
            currentUnitIndex = (currentUnitIndex + 1) % allUnits.Count;
            StartNextTurn();
        }

        public bool IsCurrentUnit(IUnit unit)
        {
            return unit == currentUnit;
        }
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public enum GameTurnState
    {
        Waiting,   // �ٸ� ������ ���� ���� ���� ����
        Active,    // ���� ���� ���� ���� ����
        Completed  // ���� ���� ����� ����
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
            // ��� ������ ����Ʈ�� �߰��ϰ� �ӵ� �������� ����
            allUnits.AddRange(FindObjectsOfType<BaseCharacterControl>());
            allUnits.AddRange(FindObjectsOfType<BaseEnemyControl>());
            allUnits.Sort((a, b) => b.UnitSpeed.CompareTo(a.UnitSpeed));

            // ù ��° ������ �� ����
            StartNextTurn();
        }

        public void StartNextTurn()
        {
            if (allUnits.Count == 0) return;

            // ��� ������ �� ���¸� ��� ���·� ����
            gameTurnState = GameTurnState.Waiting;

            currentUnit = allUnits[currentUnitIndex];

            // ���� ������ ���� Ȱ��ȭ
            gameTurnState = GameTurnState.Active;
            currentUnit.TakeTurn();
        }

        public void OnUnitTurnCompleted()
        {
            // ���� ������ �� ����
            gameTurnState = GameTurnState.Completed;

            // ���� ������ ������ ��ȯ
            currentUnitIndex = (currentUnitIndex + 1) % allUnits.Count;
            StartCoroutine(WaitAndStartNextTurn());
        }

        private IEnumerator WaitAndStartNextTurn()
        {
            yield return new WaitForSeconds(0.5f); // 0.5�� ���
            StartNextTurn();
        }

        public bool IsCurrentUnit(IUnit unit)
        {
            return unit == currentUnit;
        }
    }
}
*/
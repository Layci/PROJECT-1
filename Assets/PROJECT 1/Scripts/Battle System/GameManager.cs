using UnityEngine;
using System.Collections.Generic;

namespace Project1
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private IUnit currentUnit;
        private int currentUnitIndex = 0;
        private List<IUnit> allUnits = new List<IUnit>(); // ���� ����Ʈ

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
            // ��� ����(�÷��̾� �� ��)�� ����Ʈ�� �߰�
            allUnits.AddRange(FindObjectsOfType<BaseCharacterControl>());
            allUnits.AddRange(FindObjectsOfType<BaseEnemyControl>());

            // �ӵ� �������� ����
            allUnits.Sort((a, b) => b.UnitSpeed.CompareTo(a.UnitSpeed));

            // ù ��° ������ �� ����
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;  // ����Ʈ ������ ���� �߰�
using UnityEngine;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public List<BaseCharacterControl> playerUnits; // �÷��̾� ���� ����Ʈ
        public List<BaseEnemyControl> enemyUnits;      // �� ���� ����Ʈ
        private List<MonoBehaviour> turnOrder;         // �� ������ ���� ���� ����
        private int currentTurnIndex = 0;              // ���� ���� ���� ���� ���� �ε���

        public GameState currentState;

        private void Start()
        {
            SetupTurnOrder();  // �� ������ �ʱ�ȭ
            StartNextTurn();   // ù ��° �� ����
        }

        // �� ������ ���� (�ӵ��� ����)
        private void SetupTurnOrder()
        {
            // ��� ����(�÷��̾�� ��)�� �ϳ��� ����Ʈ�� ��ġ�� �ӵ��� ���� ����
            turnOrder = new List<MonoBehaviour>();
            turnOrder.AddRange(playerUnits);
            turnOrder.AddRange(enemyUnits);
            turnOrder = turnOrder.OrderByDescending(unit =>
                (unit is BaseCharacterControl ? ((BaseCharacterControl)unit).unitSpeed : ((BaseEnemyControl)unit).unitSpeed)
            ).ToList();
        }

        // ���� �����ϰ� ���� �� ����
        public void EndTurn()
        {
            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
            StartNextTurn();
        }

        // ���� �� ���ֿ� ���� ������ �� ����
        private void StartNextTurn()
        {
            if (turnOrder[currentTurnIndex] is BaseCharacterControl playerUnit)
            {
                StartPlayerTurn(playerUnit);  // �÷��̾� �� ����
            }
            else if (turnOrder[currentTurnIndex] is BaseEnemyControl enemyUnit)
            {
                StartEnemyTurn(enemyUnit);    // �� �� ����
            }
        }

        // �÷��̾� �� ���� (���� �÷��̾� ����)
        private void StartPlayerTurn(BaseCharacterControl playerUnit)
        {
            Debug.Log($"{playerUnit.name}�� ���Դϴ�.");
            playerUnit.WaitForInput();  // �÷��̾� �Է��� ��ٸ�
        }

        // �� �� ���� (���� �� ����)
        private void StartEnemyTurn(BaseEnemyControl enemyUnit)
        {
            Debug.Log($"{enemyUnit.name}�� ���Դϴ�.");
            enemyUnit.StartAttack();   // ���� �ڵ����� ������ ����
            EndTurn();  // ���� ���� �� ��� �� ����
        }

        // �÷��̾� ���� �����ϴ� �Լ� (���� ������ �°�)
        public void StartPlayerTurn()
        {
            if (currentState != null)
            {
                currentState.EnterState(this);
            }
        }

        public void PerformEnemyAttack()
        {
            // ���� ���� ������ ó���ϴ� �Լ�
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

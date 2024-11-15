using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public List<BaseCharacterControl> playerTeam = new List<BaseCharacterControl>();  // �÷��̾� ��
        public List<BaseEnemyControl> enemyTeam = new List<BaseEnemyControl>();           // �� ��
        private Queue<object> turnQueue = new Queue<object>();

        // ���� ���� ĳ���͸� �ܺο��� ������ �� �ֵ��� public �Ӽ����� ����
        public object CurrentCharacter { get; private set; }

        private void Start()
        {
            InitializeTurnOrder();
            StartTurn();
        }

        // �� ���� �ʱ�ȭ
        private void InitializeTurnOrder()
        {
            foreach (var player in playerTeam)
            {
                turnQueue.Enqueue(player);
            }
            foreach (var enemy in enemyTeam)
            {
                turnQueue.Enqueue(enemy);
            }
        }

        // ���� ĳ������ �� ���� (public���� ����)
        public void StartTurn()
        {
            if (turnQueue.Count > 0)
            {
                CurrentCharacter = turnQueue.Dequeue();

                if (CurrentCharacter is BaseCharacterControl playerControl)
                {
                    playerControl.WaitForInput(); // �÷��̾ �Է��� ���� �� �ֵ��� ����
                }
                else if (CurrentCharacter is BaseEnemyControl enemyControl)
                {
                    enemyControl.StartAttack(); // ���� �ڵ����� �����ϵ��� ����
                }

                // ���� ���� �� ĳ���͸� �ٽ� ť�� �߰�
                turnQueue.Enqueue(CurrentCharacter);
            }
        }

        // �� ���� �޼���
        public void EndTurn()
        {
            StartTurn();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public List<BaseCharacterControl> playerTeam = new List<BaseCharacterControl>();  // 플레이어 팀
        public List<BaseEnemyControl> enemyTeam = new List<BaseEnemyControl>();           // 적 팀
        private Queue<object> turnQueue = new Queue<object>();

        // 현재 턴의 캐릭터를 외부에서 접근할 수 있도록 public 속성으로 설정
        public object CurrentCharacter { get; private set; }

        private void Start()
        {
            InitializeTurnOrder();
            StartTurn();
        }

        // 턴 순서 초기화
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

        // 다음 캐릭터의 턴 시작 (public으로 변경)
        public void StartTurn()
        {
            if (turnQueue.Count > 0)
            {
                CurrentCharacter = turnQueue.Dequeue();

                if (CurrentCharacter is BaseCharacterControl playerControl)
                {
                    playerControl.WaitForInput(); // 플레이어가 입력을 받을 수 있도록 설정
                }
                else if (CurrentCharacter is BaseEnemyControl enemyControl)
                {
                    enemyControl.StartAttack(); // 적이 자동으로 공격하도록 설정
                }

                // 턴이 끝난 후 캐릭터를 다시 큐에 추가
                turnQueue.Enqueue(CurrentCharacter);
            }
        }

        // 턴 종료 메서드
        public void EndTurn()
        {
            StartTurn();
        }
    }
}

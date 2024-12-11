using ProJect1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem instance; // 싱글톤 인스턴스

        public int selectedEnemyIndex = 0; // 현재 선택된 적의 인덱스
        public int randomIndex; // 랜덤 캐릭터리스트 인덱스
        public List<BaseCharacterControl> playerCharacters; // 플레이어 캐릭터 리스트
        public List<BaseEnemyControl> enemyCharacters; // 적 캐릭터 리스트
        private List<object> allCharacters; // 모든 캐릭터를 포함하는 리스트
        public Transform playerTargetPosition;

        public List<BaseEnemyControl> activeEnemies = new List<BaseEnemyControl>(); // 적 타겟 리스트


        public int currentTurnIndex = 0; // 현재 턴을 담당하는 캐릭터의 인덱스

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // 모든 캐릭터를 가져와 리스트에 추가
            playerCharacters = FindObjectsOfType<BaseCharacterControl>().ToList();
            enemyCharacters = FindObjectsOfType<BaseEnemyControl>().ToList();

            // 모든 캐릭터를 하나의 리스트에 추가
            allCharacters = new List<object>();
            allCharacters.AddRange(playerCharacters);
            allCharacters.AddRange(enemyCharacters);
      
            // unitSpeed를 기준으로 내림차순 정렬
            allCharacters = allCharacters.OrderByDescending(character =>
            {
                if (character is BaseCharacterControl player)
                    return player.unitSpeed;
                else if (character is BaseEnemyControl enemy)
                    return enemy.unitSpeed;
                return 0;
            }).ToList();

            SortEnemiesByPosition();

            StartTurn(); // 첫 번째 턴 시작
        }

        // 맨 처음 실행
        private void StartTurn()
        {
            if (currentTurnIndex >= allCharacters.Count)
                currentTurnIndex = 0; // 인덱스가 리스트를 초과하면 다시 처음으로

            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = true;

                EnemySelectorUI.instance.isTurn = true;
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                enemyCharacter.isTurn = true;
            }

            RandomPlayer();
        }

        // 턴이 끝날시 호출
        public void EndTurn()
        {
            // 현재 턴 캐릭터의 isTurn을 false로 설정
            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = false;
                EnemySelectorUI.instance.isTurn = false;
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                enemyCharacter.isTurn = false;
            }

            // 다음 캐릭터로 넘어감
            currentTurnIndex++;
            if (currentTurnIndex >= allCharacters.Count)
            {
                currentTurnIndex = 0; // 인덱스가 리스트를 초과하면 다시 처음으로
            }

            StartTurn(); // 다음 턴 시작
        }

        private void SortEnemiesByPosition()
        {
            // 적 리스트를 x값 기준으로 오름차순 정렬
            enemyCharacters = enemyCharacters.OrderBy(enemy => enemy.transform.position.x).ToList();
        }

        public void RemoveCharacterFromTurnOrder(object character)
        {
            // 사망한 캐릭터를 전체 턴 리스트에서 제거
            allCharacters.Remove(character);

            // 적 캐릭터 리스트에서 제거
            if (character is BaseEnemyControl enemy)
            {
                enemyCharacters.Remove(enemy);
            }

            // 플레이어 캐릭터 리스트에서도 제거 가능
            if (character is BaseCharacterControl player)
            {
                playerCharacters.Remove(player);
            }

            // 현재 턴 인덱스가 리스트 범위를 초과하지 않도록 보정
            if (currentTurnIndex >= allCharacters.Count)
            {
                currentTurnIndex = 0;
            }
        }

        // 적 추가 함수
        public void RegisterEnemy(BaseEnemyControl enemy)
        {
            // 적 리스트에 추가
            enemyCharacters.Add(enemy);

            // 전체 턴 리스트에도 추가
            allCharacters.Add(enemy);

            // unitSpeed 기준으로 다시 정렬
            allCharacters = allCharacters.OrderByDescending(character =>
            {
                if (character is BaseCharacterControl player)
                    return player.unitSpeed;
                else if (character is BaseEnemyControl enemyControl)
                    return enemyControl.unitSpeed;
                return 0;
            }).ToList();
        }

        public void RandomPlayer()
        {
            randomIndex = Random.Range(0, playerCharacters.Count);

            playerTargetPosition = playerCharacters[randomIndex].transform;
        }
    }
}

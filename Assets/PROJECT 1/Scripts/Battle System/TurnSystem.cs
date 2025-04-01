using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem instance; // 싱글톤 인스턴스

        public int currentTurn = 0; // 현재 진행중인 턴
        public int selectedEnemyIndex = 0; // 현재 선택된 적의 인덱스
        public int randomIndex; // 랜덤 캐릭터리스트 인덱스
        public int randomPoint; // 랜덤 적 스킬포인트
        public List<BaseCharacterControl> playerCharacters; // 플레이어 캐릭터 리스트
        public List<BaseEnemyControl> enemyCharacters; // 적 캐릭터 리스트
        private List<object> allCharacters; // 모든 캐릭터를 포함하는 리스트
        public Transform playerTargetPosition;

        //public List<BaseEnemyControl> activeEnemies = new List<BaseEnemyControl>(); // 적 타겟 리스트
        public List<BaseUnit> allUnits; // 전투에 있는 모든 캐릭터 (버프 관리용 리스트)

        public int currentTurnIndex = 0; // 현재 턴을 담당하는 캐릭터의 인덱스
        public TurnOrderUI turnOrderUI;  // 턴 순서 UI 관리 스크립트

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
            allUnits = FindObjectsOfType<BaseUnit>().ToList();

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

            Debug.Log($"[StartTurn] 현재 턴 인덱스: {currentTurnIndex}");

            BaseUnit currentUnit = allCharacters[currentTurnIndex] as BaseUnit;

            if (currentUnit != null)
            {
                Debug.Log($"[OnTurnStart] {currentUnit.name}의 버프 확인 시작");

                currentUnit.OnTurnStart(); // 현재 턴 유닛의 버프 지속 턴만 감소
            }

            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = true;
                EnemySelectorUI.instance.isTurn = true;

                if (playerCharacter.isBlock)
                {
                    playerCharacter.isBlock = false;
                    playerCharacter.startBlocking = false;
                    playerCharacter.DoneBlock();
                }
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                if(enemyCharacter.enemySkillPoint >= 2)
                {
                    enemyCharacter.skillAttack = true;
                    Debug.Log("적 스킬공격");
                }
                enemyCharacter.isTurn = true;
            }

            // 랜덤 플레이어 타겟
            RandomPlayer();

            // UI 갱신
            turnOrderUI.Initialize(allCharacters, currentTurnIndex);
        }

        // 턴이 끝날시 호출
        public void EndTurn()
        {
            // 현재 턴 캐릭터의 isTurn을 false로 설정
            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = false;
                EnemySelectorUI.instance.isTurn = false;

                // BuffUI 컴포넌트 찾기
                BuffUI buffUI = playerCharacter.GetComponent<BuffUI>();
                if (buffUI != null)
                {
                    buffUI.UpdateBuffTurn(playerCharacter.buffTrun); // 남은 버프 턴 업데이트
                    Debug.Log($"{playerCharacter.name}의 BuffUI 업데이트 완료! (남은 턴: {playerCharacter.buffTrun})");
                }
                else
                {
                    Debug.LogWarning($"{playerCharacter.name}의 BuffUI를 찾을 수 없습니다!");
                }
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                enemyCharacter.isTurn = false;
                randomPoint = Random.Range(1, 3);
                enemyCharacter.enemySkillPoint += randomPoint;
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
            // isBlock이 true인 플레이어가 있는지 확인
            BaseCharacterControl blockingCharacter = playerCharacters.FirstOrDefault(player => player.isBlock);

            if (blockingCharacter != null)
            {
                // 방어 중인 캐릭터가 있으면 해당 캐릭터 우선 타격
                playerTargetPosition = blockingCharacter.transform;
            }
            else
            {
                // 방어 중인 캐릭터가 없으면 랜덤 타겟 선택
                randomIndex = Random.Range(0, playerCharacters.Count);
                playerTargetPosition = playerCharacters[randomIndex].transform;
            }
        }
    }
}

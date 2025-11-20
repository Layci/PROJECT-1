using Project1;
using ProJect1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace Project1
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem instance; // 싱글톤 인스턴스

        public int currentTurn = 1; // 현재 진행중인 턴
        public int currentWave = 0; // 현재 진행중인 웨이브
        public int selectedEnemyIndex = 0; // 현재 선택된 적의 인덱스
        public int currentPlayerTargetIndex = 0; // 현재 선택된 플레이어 인덱스
        public int randomIndex; // 랜덤 캐릭터리스트 인덱스
        public int randomPoint; // 랜덤 적 스킬포인트
        public float textDuration = 2f;
        public float fadeDuration = 1f;
        public List<BaseCharacterControl> playerCharacters; // 플레이어 캐릭터 리스트
        public List<BaseEnemyControl> enemyCharacters; // 적 캐릭터 리스트
        public List<Transform> playerPositions; // 플레이어용 포지션
        public List<Transform> enemyPositions;  // 적용 포지션
        //private List<object> allCharacters; // 모든 캐릭터를 포함하는 리스트
        public List<BaseUnit> allCharacters; // 모든 캐릭터를 포함하는 리스트
        public Transform playerTargetPosition;
        public Transform uiParent; // Canvas > CharacterUIContainer
        private EnemyWaveManager waveManager;

        public Text curTurnText;
        public Text curWaveText;
        public Text winText;

        //public List<BaseEnemyControl> activeEnemies = new List<BaseEnemyControl>(); // 적 타겟 리스트
        public List<BaseUnit> allUnits; // 전투에 있는 모든 캐릭터 (버프 관리용 리스트)

        public int currentTurnIndex = 0; // 현재 턴을 담당하는 캐릭터의 인덱스
        public TurnOrderUI turnOrderUI;  // 턴 순서 UI 관리 스크립트

        public BaseUnit CurrentCharacter
        {
            get
            {
                if (allCharacters == null || allCharacters.Count == 0) return null;
                if (currentTurnIndex < 0 || currentTurnIndex >= allCharacters.Count) return null;
                return allCharacters[currentTurnIndex];
            }
            /*get
            {
                if (allCharacters.Count == 0) return null;
                return allCharacters[currentTurnIndex];
            }*/
        }

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
            //RefreshCharacterLists(); // 캐릭터 재정렬
            waveManager = BattleManager.instance.waveManager;

            UpdateTurnUI();

            // 웨이브 스폰 완료 시 초기화 진행
            EnemyWaveManager.OnWaveSpawned += InitializeBattle;

            //StartTurn(); // 첫 번째 턴 시작
        }

        // 맨 처음 실행
        private void StartTurn()
        {
            if (currentTurnIndex >= allCharacters.Count)
            {
                currentTurnIndex = 0; // 인덱스가 리스트를 초과하면 다시 처음으로
                currentTurn++;        // 진행중인 턴 상승
                UpdateTurnUI();
            }
            ButtonManager.instance.HighlightBtn();

            Debug.Log($"[StartTurn] 현재 턴 인덱스: {currentTurnIndex}");

            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = true;
                EnemySelectorUI.instance.isTurn = true;
                EnemySelection.instance.UpdateSelectedEnemy();
                BaseUnit currentUnit = allCharacters[currentTurnIndex] as BaseUnit;
                if (currentUnit != null)
                {
                    Debug.Log($"[OnTurnStart] {currentUnit.name}의 버프 확인 시작");
                    currentUnit.OnTurnStart(); // 현재 턴 유닛의 버프 지속 턴만 감소
                }

                if (playerCharacter.ui != null)
                {
                    playerCharacter.ui.UpdateBuff();   // UI 갱신
                    Debug.Log($"{playerCharacter.name}의 캐릭터 UI에서 Buff 표시 갱신됨!");
                }

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
            if (enemyCharacters.Count <= 0)
            {
                currentWave++;
                Debug.Log($"웨이브 증가 : {currentWave}");
                // 웨이브 시작 코드
                // 웨이브가 남아 있으면 다음 웨이브 시작
                if (currentWave < waveManager.TotalWaveCount)
                {
                    waveManager.SpawnWave(currentWave);
                    currentTurnIndex = 0;
                    RefreshCharacterLists();
                    UpdateTurnUI();
                    ShowWaveStart();
                    StartTurn();
                    return;
                }
                else
                {
                    Debug.Log("모든 웨이브 완료! 전투 종료.");
                    // 전체 전투 승리 처리
                    StartCoroutine(ShowWinText());
                }
            }
            // 현재 턴 캐릭터의 isTurn을 false로 설정
            if (allCharacters[currentTurnIndex] is BaseCharacterControl playerCharacter)
            {
                playerCharacter.isTurn = false;
                EnemySelectorUI.instance.isTurn = false;
            }
            else if (allCharacters[currentTurnIndex] is BaseEnemyControl enemyCharacter)
            {
                enemyCharacter.isTurn = false;
                randomPoint = Random.Range(1, 3);
                enemyCharacter.enemySkillPoint += randomPoint;
            }

            // 다음 캐릭터로 넘어감
            currentTurnIndex++;
            
            StartTurn(); // 다음 턴 시작
        }

        private void SortEnemiesByPosition()
        {
            // 적 리스트를 x값 기준으로 오름차순 정렬
            enemyCharacters = enemyCharacters.OrderBy(enemy => enemy.transform.position.x).ToList();
        }

        private void SortPlayersByPosition()
        {
            playerCharacters = playerCharacters.OrderBy(player => player.transform.position.x).ToList();
        }

        public void RemoveCharacterFromTurnOrder(BaseUnit character)
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

        // 캐릭터리스트 재정렬 함수
        public void RefreshCharacterLists()
        {
            playerCharacters = FindObjectsOfType<BaseCharacterControl>().ToList();
            enemyCharacters = FindObjectsOfType<BaseEnemyControl>().ToList();
            allUnits = FindObjectsOfType<BaseUnit>().ToList();

            //allCharacters = new List<object>();
            allCharacters = new List<BaseUnit>();
            allCharacters.AddRange(playerCharacters);
            allCharacters.AddRange(enemyCharacters);

            // unitSpeed 기준으로 정렬
            allCharacters = allCharacters.OrderByDescending(character =>
            {
                if (character is BaseCharacterControl player)
                    return player.unitSpeed;
                else if (character is BaseEnemyControl enemy)
                    return enemy.unitSpeed;
                return 0;
            }).ToList();

            SortEnemiesByPosition(); // 위치 정렬 유지
            SortPlayersByPosition();
            turnOrderUI.Initialize(allCharacters, currentTurnIndex); // 턴 UI 갱신
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

        public void UpdateTurnUI()
        {
            curTurnText.text = ($"Turn {currentTurn}");
        }

        public void ShowWaveStart()
        {
            curWaveText.text = $"Wave {currentWave} Start!";
            curWaveText.gameObject.SetActive(true);
            StartCoroutine(HideWaveTextAfterSeconds(2f)); // 2초 후 숨김
        }

        // 모든 플레이어 캐릭터 공격 상태 변경
        public void SetAllPlayersPrepareState(AttackPrepareState state)
        {
            foreach (var unit in allCharacters)
            {
                if (unit is BaseCharacterControl player)
                {
                    player.prepareState = state;
                }
            }
        }

        private void InitializeBattle()
        {
            // 이제 스폰이 끝났으니 캐릭터들 새로 불러오기
            RefreshCharacterLists();

            currentTurnIndex = 0;
            currentTurn = 1;

            // 마우스 잠금 해제 (전투 UI 조작 위해)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log($"플레이어 캐릭터 수: {playerCharacters.Count}");
            foreach (var ch in playerCharacters)
            {
                Debug.Log($"플레이어 발견: {ch.name}");
            }
            foreach (var character in playerCharacters)
            {
                CreateUIForCharacter(character);
            }

            UpdateTurnUI();
            // 첫 턴 시작
            StartTurn();

            // 이벤트 중복 방지
            EnemyWaveManager.OnWaveSpawned -= InitializeBattle;
        }

        public void CreateUIForCharacter(BaseCharacterControl character)
        {
            /*Debug.Log($"➡ UI 생성 시도: {character.name}");
            if (character.uiPrefab == null)
            {
                Debug.LogError($" {character.name} 의 uiPrefab 이 NULL 입니다!");
                return;
            }
            if (uiParent == null)
            {
                Debug.LogError(" uiParent 가 NULL 입니다! UI를 생성할 부모가 없습니다.");
                return;
            }*/
            GameObject uiObj = Instantiate(character.uiPrefab, uiParent);
            RectTransform rt = uiObj.GetComponentInChildren<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            //Debug.Log($"Instantiate 완료: {uiObj.name}");
            CharacterUI ui = uiObj.GetComponentInChildren<CharacterUI>();
            /*if (ui == null)
            {
                Debug.LogError($"CharacterUI 컴포넌트 없음! {uiObj.name}");
                return;
            }*/
            //Debug.Log("UI실행");
            ui.Init(character);
            character.ui = ui;
            //Debug.Log($"➡ UI Init 완료: {character.name}");
        }

        IEnumerator HideWaveTextAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            curWaveText.gameObject.SetActive(false);
        }

        IEnumerator ShowWinText()
        {
            winText.gameObject.SetActive(true);
            Color originalColor = winText.color;
            originalColor.a = 1f;
            winText.color = originalColor;
            yield return new WaitForSeconds(textDuration);
            // 페이드 아웃
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                winText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            winText.gameObject.SetActive(false);
        }
    }
}

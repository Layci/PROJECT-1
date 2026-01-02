using Project1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProJect1
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;

        public EnemyWaveManager waveManager;
        public Transform[] enemySpawnPoints; // 원래 적들 위치
        public Vector3 startPlayerPos = new Vector3(-3, 0, -2); // 기준 위치
        public Vector3 startEnemyPos = new Vector3(-3, 0, 3);
        public float spacing = 2f; // 유닛 간 간격
        public int currentWave = 0;

        public List<BaseCharacterControl> playerCharacters = new List<BaseCharacterControl>();
        public List<BaseEnemyControl> enemyCharacters = new List<BaseEnemyControl>();

        private void Awake()
        {
            // 싱글톤 초기화
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            enemySpawnPoints = GameObject.FindGameObjectsWithTag("Enemy")
        .OrderBy(e => e.transform.position.x) // 정렬은 네 기준에 맞게
        .Select(e => e.transform)
        .ToArray();
        }

        private void Start()
        {
            // 편성 정보가 있다면 생성
            if (PartyFormationManager.Instance != null && PartyFormationManager.Instance.currentParty.Count > 0)
            {
                Debug.Log($"전투 시작 - EnemyID: {BattleContext.enemyID}");

                SpawnPlayerUnitsFromFormation();
                PartyFormationManager.Instance.LoadPartyState(playerCharacters); // 저장된 체력 로드
            }

            // 기존대로 유닛 리스트 갱신
            RefreshUnitLists();
            // 유닛 이펙트 정보 갱신
            RegisterAllEffects();
        }

        // 모든 캐릭터 이펙트 정보 전달
        private void RegisterAllEffects()
        {
            // 플레이어 캐릭터
            foreach (var player in playerCharacters)
            {
                RegisterUnitEffects(player);
            }

            // 적 캐릭터
            foreach (var enemy in enemyCharacters)
            {
                RegisterUnitEffects(enemy);
            }
        }

        public void OnEnemySpawnCompleted()
        {
            RefreshUnitLists();
            RegisterAllEffects();
            Debug.Log($"[Register] enemies = {enemyCharacters.Count}");
        }


        // 전달 받은 이펙트 정보를 풀 매니저에 저장
        private void RegisterUnitEffects(BaseUnit unit)
        {
            var effects = unit.GetAllEffects();
            
            foreach (var effect in effects)
            {
                EffectPoolManager.Instance.RegisterEffect(effect);
            }
        }

        public void RefreshUnitLists()
        {
            playerCharacters = FindObjectsOfType<BaseCharacterControl>()
                .OrderBy(p => p.transform.position.x)
                .ToList();

            enemyCharacters = FindObjectsOfType<BaseEnemyControl>()
                .OrderBy(e => e.transform.position.x)
                .ToList();

            Debug.Log($"[UnitManager] 아군 {playerCharacters.Count}명, 적군 {enemyCharacters.Count}명 발견됨.");
        }

        // 플레이어 포지션 재설정
        /*public void RepositionPlayerUnits()
        {
            List<BaseCharacterControl> alivePlayers = playerCharacters
                .Where(p => p.curHealth > 0)
                .ToList();

            Vector3 currentPos = startPlayerPos;

            for (int i = 0; i < alivePlayers.Count; i++)
            {
                alivePlayers[i].transform.position = currentPos;

                alivePlayers[i].initialPosition = currentPos;

                // 다음 유닛 간격 누적
                currentPos += new Vector3(alivePlayers[i].unitSpacing, 0, 0);
            }
        }*/

        public void RepositionEnemyUnits()
        {
            List<BaseEnemyControl> aliveEnemies = enemyCharacters
                .Where(e => e.curHealth > 0)
                .ToList();

            Vector3 currentPos = startEnemyPos;

            for (int i = 0; i < aliveEnemies.Count; i++)
            {
                aliveEnemies[i].transform.position = currentPos;

                // 개별 적의 초기 위치 설정
                aliveEnemies[i].initialPosition = currentPos;

                // 다음 유닛 간격 누적
                currentPos += new Vector3(aliveEnemies[i].unitSpacing, 0, 0);
            }
        }

        private void SpawnPlayerUnitsFromFormation()
        {
            Vector3 currentPos = startPlayerPos;

            foreach (var member in PartyFormationManager.Instance.currentParty)
            {
                if (member.prefab == null) continue;

                // 프리팹 생성
                GameObject player = Instantiate(member.prefab, currentPos, Quaternion.identity);
                member.battleInstance = player;
                // BaseCharacterControl 등록
                var control = player.GetComponent<BaseCharacterControl>();
                if (control != null)
                {
                    playerCharacters.Add(control);
                    control.initialPosition = currentPos;
                    /*member.maxHP = (int)control.maxHealth;
                    member.currentHP = (int)control.curHealth;*/
                }

                // 간격 적용
                currentPos += new Vector3(control.unitSpacing, 0, 0);
            }

            Debug.Log($"[BattleManager] 편성된 {playerCharacters.Count}명의 플레이어 생성 완료");
        }

        // 전투 결과 저장
        public void SaveBattleResult()
        {
            var party = PartyFormationManager.Instance.currentParty;
            if (party == null || party.Count == 0)
                return;

            // 전투 인스턴스 기준으로 각 멤버의 HP 저장
            foreach (var member in party)
            {
                // battleInstance가 존재하면 전투 인스턴스에서 정보 읽기
                if (member.battleInstance != null)
                {
                    var unit = member.battleInstance.GetComponent<BaseCharacterControl>();

                    if (unit == null)
                        continue;

                    // 죽은 경우는 1로 저장, 아니면 실제 남은 체력 저장
                    member.currentHP = unit.isDead ? 1 : (int)unit.curHealth;

                    // 최대체력도 저장해두면 나중에 사용하기 편함
                    member.maxHP = (int)unit.maxHealth;
                }
                else
                {
                    // 안전장치: battleInstance가 없을 때는 prefab의 기본값을 보존하거나 갱신
                    var prefabStats = member.prefab.GetComponent<BaseCharacterControl>();
                    if (prefabStats != null)
                    {
                        // 전투 중 instance가 존재하지 않으면 prefab 기준으로 세팅
                        // (보통의 경우 SaveBattleResult는 전투 종료시에 호출되므로 여기로 오면 드문 케이스)
                        member.maxHP = (int)prefabStats.maxHealth;
                        // member.currentHP를 변경하지 않음(이미 값이 있을 수 있으므로)
                    }
                }
            }
        }

        public void OnBattleWin()
        {
            BattleTransitionManager.Instance.EndBattle();
        }
    }
}

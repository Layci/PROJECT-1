using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace ProJect1
{
    /*[System.Serializable]
    public class PartyMemberState
    {
        public string characterName;
        public int currentHP;
        public int maxHP;

        public PartyMemberState(string name, int hp, int max)
        {
            characterName = name;
            currentHP = hp;
            maxHP = max;
        }
    }*/
    [System.Serializable]
    public class PartyMemberData
    {
        public GameObject prefab;     // 전투씬에서 Instantiate할 프리팹
        public string characterName;  // 캐릭터 이름(선택적으로 UI용)
        public GameObject battleInstance; // 전투에서 Instantiate된 오브젝트
        public Sprite icon;           // 캐릭터 아이콘
        public int currentHP;
        public int maxHP;
    }

    public class PartyFormationManager : MonoBehaviour
    {
        public static PartyFormationManager Instance;

        public List<PartyMemberData> currentParty = new(); // 현재 파티 저장 목록
        // 111111111111111111
        //public List<PartyMemberState> partyStates = new(); // 전투 결과 저장 목록
        //public Dictionary<string, PartyMemberState> allStates = new Dictionary<string, PartyMemberState>();
        public Vector3 lastFieldPosition; // 전투 전 플레이어 필드 위치
        public int maxPartySize = 4;

        // 중복 선택 방지 기능 ON/OFF
        public bool preventDuplicate = true;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            BuildPartyStates();
        }

        /*public void SetParty(List<PartyMemberData> newParty)
        {
            currentParty = newParty;
        }*/

        // 슬롯에 캐릭터 배정
        public void SetSlot(int index, PartyMemberData data)
        {
            if (index >= currentParty.Count)
                currentParty.Add(data);
            else
                currentParty[index] = data;
        }

        // 슬롯 비우기
        public void RemoveSlot(int index)
        {
            if (index < currentParty.Count)
                currentParty.RemoveAt(index);
        }

        public void ResetParty()
        {
            for (int i = 0; i < currentParty.Count; i++)
                currentParty[i] = null;
        }

        // 파티 중복 여부 확인
        public bool IsCharacterAlreadySelected(PartyMemberData data)
        {
            foreach (var member in currentParty)
            {
                if (member == data) return true;
            }
            return false;
        }

        public void BuildPartyStates()
        {
            foreach (var member in currentParty)
            {
                BaseCharacterControl baseStats = member.prefab.GetComponent<BaseCharacterControl>();

                // 최대 체력 세팅
                member.maxHP = (int)baseStats.maxHealth;

                // 현재 체력을 풀피로 초기화
                member.currentHP = member.maxHP;
            }
        }

        // 111111111111111
        /*public void BuildPartyStates()
        {
            partyStates.Clear();

            foreach (var member in currentParty)
            {
                // 1) 프리팹에서 BaseCharacterControl 가져오기 (프리팹이니까 Instantiate 필요 없음)
                BaseCharacterControl baseStats = member.prefab.GetComponent<BaseCharacterControl>();

                PartyMemberState state = new PartyMemberState(baseStats.unitName,(int)baseStats.maxHealth, (int)baseStats.maxHealth);

                partyStates.Add(state);
            }
        }*/

        // 체력 저장 방식
        /*public PartyMemberState GetOrCreateState(PartyMemberData member)
        {
            if (allStates.TryGetValue(member.characterName, out var state))
                return state;  // 기존 체력 유지

            BaseCharacterControl stats = member.prefab.GetComponent<BaseCharacterControl>();

            state = new PartyMemberState(stats.unitName, (int)stats.maxHealth, (int)stats.maxHealth);

            allStates.Add(member.characterName, state);

            return state;
        }

        public void ReBuildPartyStates()
        {
            partyStates.Clear();

            foreach (var member in currentParty)
            {
                var state = GetOrCreateState(member);
                partyStates.Add(state);
            }
        }*/

        public void RebuildPartyData()
        {
            foreach (var member in currentParty)
            {
                // Base stats 가져오기
                var baseStats = member.prefab.GetComponent<BaseCharacterControl>();
                int prefabMax = (int)baseStats.maxHealth;

                // maxHP가 0이면 프리팹 기준으로 세팅(초기화 안 된 경우)
                if (member.maxHP == 0)
                    member.maxHP = prefabMax;

                // currentHP가 0이면 "아직 값이 없음"으로 보고 풀피로 세팅
                // (주의: 0을 '사망'으로 쓰고 있다면 이 로직을 바꿔야 함)
                if (member.currentHP == 0)
                    member.currentHP = 1;
            }

            Debug.Log("Party data rebuilt (currentHP/maxHP 보정 완료)");
        }
        // 11111111111111111111111
        /*public void ReBuildPartyStates()
        {
            List<PartyMemberState> newStates = new List<PartyMemberState>();

            foreach (var member in currentParty)
            {
                // 기존에 저장된 체력/상태 찾기
                PartyMemberState existing = partyStates.Find(s => s.characterName == member.characterName);

                if (existing != null)
                {
                    // 기존 체력 유지
                    newStates.Add(new PartyMemberState(
                        existing.characterName,
                        existing.currentHP,
                        existing.maxHP
                    ));
                }
                else
                {
                    // 새로 추가된 캐릭터: full hp로 생성
                    BaseCharacterControl baseStats = member.prefab.GetComponent<BaseCharacterControl>();

                    newStates.Add(new PartyMemberState(
                        baseStats.unitName,
                        (int)baseStats.maxHealth,
                        (int)baseStats.maxHealth
                    ));
                }
            }

            // 완전히 덮어쓰기
            partyStates = newStates;

            Debug.Log("PartyState 재정렬 완료 (체력 유지됨)");
        }*/

        // 111111111111111111
        public void PartyHeal()
        {
            foreach (var state in currentParty)
            {
                state.currentHP = state.maxHP;
            }

            Debug.Log("파티 전체 회복 완료!");
        }

        /*public void SavePartyState()
        {
            var manager = PartyFormationManager.Instance;

            foreach (var member in currentParty)
            {
                string key = member.prefab.GetComponent<BaseCharacterControl>().unitName;

                if (manager.allStates.TryGetValue(key, out var state))
                {
                    // 프리팹에 curHealth 적용
                    var unit = member.prefab.GetComponent<BaseCharacterControl>();
                    unit.curHealth = state.currentHP;

                    Debug.Log($"파티 상태 로드: {key}, 체력 = {state.currentHP}");

                    ReBuildPartyStates();
                }
            }
        }*/

        public void SavePartyState()
        {
            // currentParty가 비어있으면 저장할 것이 없음
            if (currentParty == null || currentParty.Count == 0)
                return;

            for (int i = 0; i < currentParty.Count; i++)
            {
                // 전투에서 생성된 인스턴스(죽었을 수도 있음)
                var instance = currentParty[i].battleInstance;

                // battleInstance가 null이면 (전투 중 제외된 경우)
                // prefab의 BaseCharacterControl로 접근
                BaseCharacterControl unit;

                if (instance != null)
                    unit = instance.GetComponent<BaseCharacterControl>();
                else
                    unit = currentParty[i].prefab.GetComponent<BaseCharacterControl>();

                // 이미 SaveBattleResult()에서 curHealth가 덮어져 있으므로
                // 여기선 따로 HP를 수정할 필요 없음.
                // prefab에 curHealth를 반영.

                var prefabStats = currentParty[i].prefab.GetComponent<BaseCharacterControl>();
                prefabStats.curHealth = unit.curHealth;
            }
        }
        // 11111111111111111111111
        /*public void SavePartyState()
        {
            var states = partyStates;

            if (states == null || states.Count == 0)
                return;

            for (int i = 0; i < currentParty.Count; i++)
            {
                var state = states[i];
                var unit = currentParty[i].prefab.GetComponent<BaseCharacterControl>();

                unit.curHealth = state.currentHP;
            }
        }*/

        // BattleManager start 함수에 위치 배틀씬 진입시 편성정보에 있는 캐릭터의 체력을 로드
        public void LoadPartyState(List<BaseCharacterControl> players)
        {
            var party = PartyFormationManager.Instance.currentParty;

            if (party == null || party.Count == 0)
                return;

            for (int i = 0; i < players.Count; i++)
            {
                var saved = party[i];      // PartyMemberData (저장된 데이터)
                var unit = players[i];     // 전투 인스턴스 BaseCharacterControl

                // 프리팹의 초기값이 아니라 PartyMemberData 값 그대로 강제 적용
                unit.curHealth = saved.currentHP;

                // maxHealth는 캐릭터 자체 스탯이므로 일반적으로 덮어쓰면 안 됨
                // unit.maxHealth = saved.maxHP;  // 웬만하면 주석!

                Debug.Log($"[LoadHP] {saved.characterName} HP = {unit.curHealth}");

                if (unit.curHealth <= 0)
                    unit.curHealth = 1;
            }
        }
        /*public void LoadPartyState(List<BaseCharacterControl> players)
        {
            var party = PartyFormationManager.Instance.currentParty;

            if (party == null || party.Count == 0)
                return;

            for (int i = 0; i < players.Count; i++)
            {
                var member = party[i];
                var unit = players[i];

                // currentParty → 전투 인스턴스 BaseCharacterControl 체력 복구
                unit.curHealth = member.currentHP;
                unit.maxHealth = member.maxHP;
                Debug.Log($"[LoadHP] {member.characterName} cur={member.currentHP} / max={member.maxHP}");
                // 체력이 0 이하라면 죽은 상태였던 것이므로 바로 1로 회복
                if (unit.curHealth <= 0)
                    unit.curHealth = 1;
            }
        }*/
        // 111111111111111111111
        /*public void LoadPartyState(List<BaseCharacterControl> players)
        {
            if (partyStates == null || partyStates.Count == 0)
                return;

            for (int i = 0; i < players.Count; i++)
            {
                var state = partyStates[i];
                var unit = players[i];

                unit.curHealth = state.currentHP;
                unit.maxHealth = state.maxHP;
                //unit.UpdateHPUI();
            }
        }*/

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainScene")
            {
                var player = GameObject.FindWithTag("Player");
                var cc = player.GetComponent<CharacterController>();
                
                if (player != null)
                {
                    if (cc != null) cc.enabled = false;
                    player.transform.position = lastFieldPosition;
                    if (cc != null) cc.enabled = true;
                }
            }
        }
        /*public void LoadPartyState(List<GameObject> spawnedParty)
        {
            var states = PartyFormationManager.Instance.partyStates;

            for (int i = 0; i < spawnedParty.Count; i++)
            {
                var unit = spawnedParty[i].GetComponent<BaseCharacterControl>();
                var state = states[i];

                unit.curHealth = state.currentHP;
                unit.maxHealth = state.maxHP;
                //unit.UpdateHPUI();
            }
        }*/
    }
}

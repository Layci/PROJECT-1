using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace ProJect1
{
    [System.Serializable]
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
    }
    [System.Serializable]
    public class PartyMemberData
    {
        public GameObject prefab;     // 전투씬에서 Instantiate할 프리팹
        public string characterName;  // 캐릭터 이름(선택적으로 UI용)
        public Sprite icon;           // 캐릭터 아이콘
    }

    public class PartyFormationManager : MonoBehaviour
    {
        public static PartyFormationManager Instance;

        public List<PartyMemberData> currentParty = new(); // 현재 파티 저장 목록
        public List<PartyMemberState> partyStates = new(); // 전투 결과 저장 목록
        public Dictionary<string, PartyMemberState> allStates = new Dictionary<string, PartyMemberState>();
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
            partyStates.Clear();

            foreach (var member in currentParty)
            {
                // 1) 프리팹에서 BaseCharacterControl 가져오기 (프리팹이니까 Instantiate 필요 없음)
                BaseCharacterControl baseStats = member.prefab.GetComponent<BaseCharacterControl>();

                PartyMemberState state = new PartyMemberState(baseStats.unitName,(int)baseStats.maxHealth, (int)baseStats.maxHealth);

                partyStates.Add(state);
            }
        }

        public PartyMemberState GetOrCreateState(PartyMemberData member)
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
        }

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

        public void PartyHeal()
        {
            foreach (var state in partyStates)
            {
                state.currentHP = state.maxHP;
            }

            Debug.Log("파티 전체 회복 완료!");
        }

        public void SavePartyState()
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
        }

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
                Debug.Log(state.currentHP);
                Debug.Log(state.characterName);
            }
        }*/

        public void LoadPartyState(List<BaseCharacterControl> players)
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
        }

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

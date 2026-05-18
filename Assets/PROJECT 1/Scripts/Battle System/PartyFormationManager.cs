using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace ProJect1
{
    [System.Serializable]
    public class PartyMemberData
    {
        public GameObject prefab;         // 캐릭터 프리팹 (생성용)
        public string characterName;      // 캐릭터 이름
        public GameObject battleInstance; // 전투 중 생성된 캐릭터 오브젝트 인스턴스
        public Sprite icon;               // 캐릭터 UI 아이콘
        public int currentHP;             // 저장된 현재 체력
        public int maxHP;                 // 캐릭터의 최대 체력
    }

    public class PartyFormationManager : MonoBehaviour
    {
        public static PartyFormationManager Instance;

        // 1. 이벤트 선언 (누구든 구독할 수 있는 알림판)
        public event Action OnPartyCompositionChanged;
        public List<PartyMemberData> currentParty = new(); // 현재 파티에 편성된 멤버 리스트
        public Vector3 lastFieldPosition; // 전투 진입 전 메인 씬에서의 마지막 플레이어 위치
        public int maxPartySize = 4;      // 최대 파티 인원수

        // 파티원 중복 선택 방지 플래그
        public bool preventDuplicate = true;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
                SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로딩 완료 이벤트 등록
                BuildPartyStates(); // 초기 파티 상태 구축
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // 파티 슬롯에 멤버 데이터 설정
        public void SetSlot(int index, PartyMemberData data)
        {
            if (index >= currentParty.Count)
                currentParty.Add(data);
            else
                currentParty[index] = data;

            OnPartyCompositionChanged?.Invoke();
        }

        // 파티 슬롯에서 멤버 제거
        public void RemoveSlot(int index)
        {
            if (index < currentParty.Count)
                currentParty.RemoveAt(index);

            OnPartyCompositionChanged?.Invoke();
        }

        // 파티 전체 초기화
        public void ResetParty()
        {
            currentParty.Clear();
            OnPartyCompositionChanged?.Invoke();
        }

        // 특정 캐릭터가 이미 파티에 포함되어 있는지 확인
        public bool IsCharacterAlreadySelected(PartyMemberData data)
        {
            foreach (var member in currentParty)
            {
                if (member == data) return true;
            }
            return false;
        }

        // 파티 멤버들의 프리팹 정보를 바탕으로 초기 체력 및 상태 구축
        public void BuildPartyStates()
        {
            foreach (var member in currentParty)
            {
                if (member.prefab == null) continue;
                BaseCharacterControl baseStats = member.prefab.GetComponent<BaseCharacterControl>();

                member.maxHP = (int)baseStats.maxHealth;
                member.currentHP = member.maxHP; // 처음에는 풀피로 시작
            }
        }

        // 파티 데이터 재구축 및 비정상 수치(체력 0 등) 보정
        public void RebuildPartyData()
        {
            foreach (var member in currentParty)
            {
                if (member.prefab == null) continue;
                var baseStats = member.prefab.GetComponent<BaseCharacterControl>();
                int prefabMax = (int)baseStats.maxHealth;

                if (member.maxHP == 0)
                    member.maxHP = prefabMax;

                if (member.currentHP == 0)
                    member.currentHP = 1; // 사망 상태가 아니라면 최소 1로 보정
            }

            Debug.Log("Party data rebuilt (체력 및 최대치 수치 보정 완료)");
            OnPartyCompositionChanged?.Invoke();
        }

        // 파티원 전체 체력 회복
        public void PartyHeal()
        {
            foreach (var state in currentParty)
            {
                state.currentHP = state.maxHP;
            }

            Debug.Log("파티 전체 회복 완료!");
        }

        // 전투 종료 후 또는 필요 시 현재 파티의 체력 상태를 프리팹/데이터에 저장
        public void SavePartyState()
        {
            if (currentParty == null || currentParty.Count == 0)
                return;

            for (int i = 0; i < currentParty.Count; i++)
            {
                var instance = currentParty[i].battleInstance;
                BaseCharacterControl unit;

                if (instance != null)
                    unit = instance.GetComponent<BaseCharacterControl>();
                else
                    unit = currentParty[i].prefab.GetComponent<BaseCharacterControl>();

                // 프리팹 데이터에도 현재 체력을 동기화
                var prefabStats = currentParty[i].prefab.GetComponent<BaseCharacterControl>();
                prefabStats.curHealth = unit.curHealth;
            }
        }

        // 전투 시작 시 저장된 체력 데이터를 생성된 전투 유닛들에 로드
        public void LoadPartyState(List<BaseCharacterControl> players)
        {
            var party = currentParty;

            if (party == null || party.Count == 0)
                return;

            for (int i = 0; i < players.Count; i++)
            {
                if (i >= party.Count) break;
                
                var saved = party[i];
                var unit = players[i];

                unit.curHealth = saved.currentHP;

                Debug.Log($"[체력 로드] {saved.characterName} HP = {unit.curHealth}");

                // 체력이 0 이하인 경우 최소 1로 보정하여 생성
                if (unit.curHealth <= 0)
                    unit.curHealth = 1;
            }
        }

        // 씬 로딩 완료 시 호출되는 콜백 함수
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainScene")
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    // 자식 오브젝트에 CharacterController가 있는 경우를 위해 GetInChildren 사용
                    var cc = player.GetComponentInChildren<CharacterController>();
                    
                    // transform.position 변경 시 충돌 연산 간섭을 막기 위해 잠시 끔
                    if (cc != null) cc.enabled = false;
                    player.transform.position = lastFieldPosition;
                    if (cc != null) cc.enabled = true;
                }
            }
        }
    }
}

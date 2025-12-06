using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void SavePartyState()
        {
            var states = PartyFormationManager.Instance.partyStates;

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
        }

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

using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public class PartyFormationManager : MonoBehaviour
    {
        public static PartyFormationManager Instance;
        public List<PartyMemberData> currentParty = new(); // 현재 파티 저장 목록
        public List<PartyMemberState> partyStates = new(); // 전투 결과 저장 목록
        public Vector3 lastFieldPosition; // 전투 전 플레이어 필드 위치

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
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

        public void SetParty(List<PartyMemberData> newParty)
        {
            currentParty = newParty;
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

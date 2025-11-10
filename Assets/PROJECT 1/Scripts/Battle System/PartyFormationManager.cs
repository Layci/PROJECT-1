using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [System.Serializable]
    public class PartyMemberData
    {
        public GameObject prefab;     // 전투씬에서 Instantiate할 프리팹
        public string characterName;  // 캐릭터 이름(선택적으로 UI용)
    }

    public class PartyFormationManager : MonoBehaviour
    {
        public static PartyFormationManager Instance;
        public List<PartyMemberData> currentParty = new();

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

        public void SetParty(List<PartyMemberData> newParty)
        {
            currentParty = newParty;
        }
    }
}
